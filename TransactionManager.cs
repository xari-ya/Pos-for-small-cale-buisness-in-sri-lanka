// File: TransactionManager.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace billing_system
{
    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }

    public class TransactionManager
    {
        // --- PROPERTIES ---
        public List<BillItem> CurrentBillItems { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal TaxTotal { get; private set; } // Kept for schema consistency, but will always be 0
        public decimal DiscountTotal { get; private set; }
        public decimal GrandTotal { get; private set; }
        public string InvoiceNumber { get; private set; }

        // --- CONSTRUCTOR ---
        public TransactionManager()
        {
            CurrentBillItems = new List<BillItem>();
            GenerateNewInvoiceNumber();
        }

        // --- PUBLIC METHODS ---

        public List<Product> SearchProducts(string searchText)
        {
            var products = new List<Product>();
            var query = "SELECT product_id, sku, name, unit_price, is_active FROM Products WHERE name LIKE @search AND is_active = 1";

            using (var conn = Database.GetConnection())
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Sku = reader.GetString(1),
                            Name = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            IsActive = reader.GetBoolean(4)
                        });
                    }
                }
            }
            return products;
        }

        public void AddItemToBill(Product product, int quantity)
        {
            var existingItem = CurrentBillItems.FirstOrDefault(item => item.ProductDetails.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new BillItem
                {
                    ProductDetails = product,
                    Quantity = quantity,
                    UnitPriceAtSale = product.Price,
                    DiscountAmount = 0,
                    TaxRatePercent = 0
                };
                CurrentBillItems.Add(newItem);
            }
            CalculateTotals();
        }

        public void RemoveItemFromBill(BillItem item)
        {
            CurrentBillItems.Remove(item);
            CalculateTotals();
        }

        public void ApplyDiscount(decimal value, DiscountType type)
        {
            if (Subtotal <= 0)
            {
                DiscountTotal = 0;
                return;
            }

            if (type == DiscountType.Percentage)
            {
                DiscountTotal = Subtotal * (value / 100m);
            }
            else // FixedAmount
            {
                DiscountTotal = value;
            }

            if (DiscountTotal > Subtotal)
            {
                DiscountTotal = Subtotal;
            }

            if (DiscountTotal < 0)
            {
                DiscountTotal = 0;
            }

            CalculateTotals();
        }

        public void ClearTransaction()
        {
            CurrentBillItems.Clear();
            DiscountTotal = 0;
            CalculateTotals();
            GenerateNewInvoiceNumber();
        }

        public (bool Success, string ErrorMessage) SaveTransactionToDatabase(string paymentMethod, decimal amountPaid)
        {
            if (AppSession.CurrentUser == null) return (false, "No user logged in.");
            if (!CurrentBillItems.Any()) return (false, "Cannot finalize an empty bill.");

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // 1) Stock checks (same as before)
                foreach (var item in CurrentBillItems)
                {
                    var stockCheckCmd = new SQLiteCommand("SELECT IFNULL(SUM(qty_change),0) FROM StockMovements WHERE product_id=@id", conn);
                    stockCheckCmd.Parameters.AddWithValue("@id", item.ProductDetails.Id);
                    var currentStock = Convert.ToInt32(stockCheckCmd.ExecuteScalar() ?? 0);
                    if (currentStock < item.Quantity)
                        return (false, $"Not enough stock for {item.ProductDetails.Name}. Only {currentStock} available.");
                }

                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // 2) Insert invoice header (invoice_no is the PK now)
                        using (var cmd = new SQLiteCommand(@"
                    INSERT INTO Invoices (invoice_no, subtotal, discount_total, tax_total, grand_total, created_by, status)
                    VALUES (@no, @sub, @disc, @tax, @grand, @user, 'PAID');", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", InvoiceNumber);
                            cmd.Parameters.AddWithValue("@sub", Subtotal);
                            cmd.Parameters.AddWithValue("@disc", DiscountTotal);
                            cmd.Parameters.AddWithValue("@tax", TaxTotal); // 0
                            cmd.Parameters.AddWithValue("@grand", GrandTotal);
                            cmd.Parameters.AddWithValue("@user", AppSession.CurrentUser.UserId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3) Insert each item (invoice_item_id = invoiceNo_seq)
                        int itemSeq = GetNextItemSeqForInvoice(conn, tx, InvoiceNumber); // starts from 1 for a new invoice
                        foreach (var item in CurrentBillItems)
                        {
                            string itemId = $"{InvoiceNumber}_{itemSeq.ToString("D4")}";
                            using (var itemCmd = new SQLiteCommand(@"
                        INSERT INTO InvoiceItems
                          (invoice_item_id, invoice_no, product_id, qty, unit_price_at_sale, discount_amount, tax_rate_percent, line_total)
                        VALUES
                          (@id, @no, @pid, @qty, @price, 0, 0, @line);", conn, tx))
                            {
                                itemCmd.Parameters.AddWithValue("@id", itemId);
                                itemCmd.Parameters.AddWithValue("@no", InvoiceNumber);
                                itemCmd.Parameters.AddWithValue("@pid", item.ProductDetails.Id);
                                itemCmd.Parameters.AddWithValue("@qty", item.Quantity);
                                itemCmd.Parameters.AddWithValue("@price", item.UnitPriceAtSale);
                                itemCmd.Parameters.AddWithValue("@line", item.LineTotal);
                                itemCmd.ExecuteNonQuery();
                            }

                            // 4) Stock movement (movement_id app-generated; ref by invoice_no)
                            string moveId = GenerateDailyId(conn, tx, "StockMovements", "movement_id", "M");
                            using (var mvCmd = new SQLiteCommand(@"
                        INSERT INTO StockMovements (movement_id, product_id, movement_type, qty_change, reason, ref_invoice_no)
                        VALUES (@mid, @pid, 'SALE', @chg, 'Sold via invoice', @ino);", conn, tx))
                            {
                                mvCmd.Parameters.AddWithValue("@mid", moveId);
                                mvCmd.Parameters.AddWithValue("@pid", item.ProductDetails.Id);
                                mvCmd.Parameters.AddWithValue("@chg", -item.Quantity);
                                mvCmd.Parameters.AddWithValue("@ino", InvoiceNumber);
                                mvCmd.ExecuteNonQuery();
                            }

                            itemSeq++;
                        }

                        // 5) Payment (payment_id like P yymmdd NNNN)
                        string paymentId = GenerateDailyId(conn, tx, "Payments", "payment_id", "P");
                        using (var payCmd = new SQLiteCommand(@"
                    INSERT INTO Payments (payment_id, invoice_no, amount, method)
                    VALUES (@pid, @no, @amt, @m);", conn, tx))
                        {
                            payCmd.Parameters.AddWithValue("@pid", paymentId);
                            payCmd.Parameters.AddWithValue("@no", InvoiceNumber);
                            payCmd.Parameters.AddWithValue("@amt", GrandTotal);
                            payCmd.Parameters.AddWithValue("@m", paymentMethod);
                            payCmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return (true, null);
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Console.WriteLine(ex);
                        return (false, "An error occurred while saving the invoice.");
                    }
                }
            }
        }

        /* Get next line-sequence for an invoice (0001, 0002, …) */
        private int GetNextItemSeqForInvoice(SQLiteConnection conn, SQLiteTransaction tx, string invoiceNo)
        {
            using var cmd = new SQLiteCommand(@"
        SELECT MAX(substr(invoice_item_id, instr(invoice_item_id, '_') + 1))
        FROM InvoiceItems
        WHERE invoice_no=@no;", conn, tx);
            cmd.Parameters.AddWithValue("@no", invoiceNo);
            var maxSuffix = cmd.ExecuteScalar() as string;
            if (string.IsNullOrEmpty(maxSuffix)) return 1;
            if (int.TryParse(maxSuffix, out int n)) return n + 1;
            return 1;
        }

        /* Generate daily IDs like:
           prefix='P'  -> P yymmdd NNNN  (Payments)
           prefix='M'  -> M yymmdd NNNN  (StockMovements)
        */
        private string GenerateDailyId(SQLiteConnection conn, SQLiteTransaction tx, string table, string column, string prefix)
        {
            string day = DateTime.Now.ToString("yyMMdd");
            string like = prefix + day + "%";
            using var cmd = new SQLiteCommand($@"
        SELECT MAX({column}) FROM {table} WHERE {column} LIKE @like;", conn, tx);
            cmd.Parameters.AddWithValue("@like", like);
            var last = cmd.ExecuteScalar() as string;
            int next = 1;
            if (!string.IsNullOrEmpty(last))
            {
                string suffix = last.Substring(prefix.Length + day.Length); // after yymmdd
                if (int.TryParse(suffix, out var n)) next = n + 1;
            }
            return prefix + day + next.ToString("D4");
        }


        // --- PRIVATE HELPER METHODS ---

        private void CalculateTotals()
        {
            Subtotal = CurrentBillItems.Sum(item => item.LineTotal);

            if (DiscountTotal > Subtotal)
            {
                DiscountTotal = Subtotal;
            }

            TaxTotal = 0; // Tax is always zero
            GrandTotal = Subtotal - DiscountTotal;
        }

        private void GenerateNewInvoiceNumber()
        {
            string prefix = DateTime.Now.ToString("yyMMdd");
            string newInvoiceNumber;
            var query = "SELECT MAX(invoice_no) FROM Invoices WHERE invoice_no LIKE @prefix";

            using (var conn = Database.GetConnection())
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@prefix", $"{prefix}%");
                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string lastInvoice = result.ToString();
                    string sequenceStr = lastInvoice.Substring(6);
                    int nextSequence = int.Parse(sequenceStr) + 1;
                    newInvoiceNumber = $"{prefix}{nextSequence:D4}";
                }
                else
                {
                    newInvoiceNumber = $"{prefix}0001";
                }
            }
            InvoiceNumber = newInvoiceNumber;
        }
    }
}