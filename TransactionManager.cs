using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace billing_system
{
    /// <summary>
    /// Defines the types of discounts that can be applied to a transaction.
    /// </summary>
    public enum DiscountType
    {
        /// <summary>A discount calculated as a percentage of the subtotal.</summary>
        Percentage,
        /// <summary>A fixed monetary amount deducted from the subtotal.</summary>
        FixedAmount
    }

    /// <summary>
    /// Manages the state and operations for a single sales transaction.
    /// </summary>
    /// <remarks>
    /// This class is the core of the point-of-sale logic. It handles the bill's contents, calculates totals,
    /// applies discounts, and is responsible for saving the completed transaction to the database. An instance
    /// of this class represents one customer transaction from start to finish.
    /// </remarks>
    public class TransactionManager
    {
        /// <summary>Gets the list of items currently in the bill.</summary>
        public List<BillItem> CurrentBillItems { get; private set; }
        /// <summary>Gets the total price of all items before discounts or taxes.</summary>
        public decimal Subtotal { get; private set; }
        /// <summary>Gets the total tax amount (currently unused, always 0).</summary>
        public decimal TaxTotal { get; private set; }
        /// <summary>Gets the total discount amount applied to the transaction.</summary>
        public decimal DiscountTotal { get; private set; }
        /// <summary>Gets the final amount due after discounts.</summary>
        public decimal GrandTotal { get; private set; }
        /// <summary>Gets the unique invoice number for the current transaction.</summary>
        public string InvoiceNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionManager"/> class.
        /// </summary>
        public TransactionManager()
        {
            CurrentBillItems = new List<BillItem>();
            GenerateNewInvoiceNumber();
        }

        /// <summary>
        /// Searches for active products by name.
        /// </summary>
        /// <param name="searchText">The text to search for in product names.</param>
        /// <returns>A list of matching <see cref="Product"/> objects.</returns>
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
                        products.Add(new Product { Id = reader.GetInt32(0), Sku = reader.GetString(1), Name = reader.GetString(2), Price = reader.GetDecimal(3), IsActive = reader.GetBoolean(4) });
                    }
                }
            }
            return products;
        }

        /// <summary>
        /// Adds a product to the current bill or increases its quantity if it already exists.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        public void AddItemToBill(Product product, int quantity)
        {
            var existingItem = CurrentBillItems.FirstOrDefault(item => item.ProductDetails.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new BillItem { ProductDetails = product, Quantity = quantity, UnitPriceAtSale = product.Price, DiscountAmount = 0, TaxRatePercent = 0 };
                CurrentBillItems.Add(newItem);
            }
            CalculateTotals();
        }

        /// <summary>
        /// Removes an item from the current bill.
        /// </summary>
        /// <param name="item">The bill item to remove.</param>
        public void RemoveItemFromBill(BillItem item)
        {
            CurrentBillItems.Remove(item);
            CalculateTotals();
        }

        /// <summary>
        /// Applies a discount to the entire transaction.
        /// </summary>
        /// <param name="value">The discount value (either a percentage or a fixed amount).</param>
        /// <param name="type">The type of discount to apply.</param>
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
            else
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

        /// <summary>
        /// Clears all items and discounts from the current transaction and generates a new invoice number.
        /// </summary>
        public void ClearTransaction()
        {
            CurrentBillItems.Clear();
            DiscountTotal = 0;
            CalculateTotals();
            GenerateNewInvoiceNumber();
        }

        /// <summary>
        /// Saves the entire transaction to the database within a single database transaction.
        /// </summary>
        /// <param name="paymentMethod">The method of payment (e.g., "Cash", "Card").</param>
        /// <param name="amountPaid">The amount paid by the customer.</param>
        /// <returns>A tuple indicating success and an error message if applicable.</returns>
        /// <remarks>
        /// This method performs several steps:
        /// 1. Checks available stock for all items.
        /// 2. Inserts a record into the 'Invoices' table.
        /// 3. Inserts a record for each item into the 'InvoiceItems' table.
        /// 4. Creates a 'SALE' record in the 'StockMovements' table for each item.
        /// 5. Inserts a record into the 'Payments' table.
        /// All operations are wrapped in a transaction to ensure data integrity.
        /// </remarks>
        public (bool Success, string ErrorMessage) SaveTransactionToDatabase(string paymentMethod, decimal amountPaid)
        {
            if (AppSession.CurrentUser == null) return (false, "No user logged in.");
            if (!CurrentBillItems.Any()) return (false, "Cannot finalize an empty bill.");
            using (var conn = Database.GetConnection())
            {
                conn.Open();
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
                        using (var cmd = new SQLiteCommand(@"
                    INSERT INTO Invoices (invoice_no, subtotal, discount_total, tax_total, grand_total, created_by, status)
                    VALUES (@no, @sub, @disc, @tax, @grand, @user, 'PAID');", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", InvoiceNumber);
                            cmd.Parameters.AddWithValue("@sub", Subtotal);
                            cmd.Parameters.AddWithValue("@disc", DiscountTotal);
                            cmd.Parameters.AddWithValue("@tax", TaxTotal);
                            cmd.Parameters.AddWithValue("@grand", GrandTotal);
                            cmd.Parameters.AddWithValue("@user", AppSession.CurrentUser.UserId);
                            cmd.ExecuteNonQuery();
                        }
                        int itemSeq = GetNextItemSeqForInvoice(conn, tx, InvoiceNumber);
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

        /// <summary>
        /// Gets the next sequential number for an item within a given invoice.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An active <see cref="SQLiteTransaction"/>.</param>
        /// <param name="invoiceNo">The invoice number.</param>
        /// <returns>The next item sequence number (starting from 1).</returns>
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

        /// <summary>
        /// Generates a unique, date-based sequential ID for a given table and prefix.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An active <see cref="SQLiteTransaction"/>.</param>
        /// <param name="table">The name of the table.</param>
        /// <param name="column">The name of the ID column.</param>
        /// <param name="prefix">The prefix for the ID (e.g., "P" for Payments).</param>
        /// <returns>A unique ID string (e.g., "P2510240001").</returns>
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
                string suffix = last.Substring(prefix.Length + day.Length);
                if (int.TryParse(suffix, out var n)) next = n + 1;
            }
            return prefix + day + next.ToString("D4");
        }

        /// <summary>
        /// Recalculates the subtotal, discount, and grand total for the current bill.
        /// </summary>
        private void CalculateTotals()
        {
            Subtotal = CurrentBillItems.Sum(item => item.LineTotal);
            if (DiscountTotal > Subtotal)
            {
                DiscountTotal = Subtotal;
            }
            TaxTotal = 0;
            GrandTotal = Subtotal - DiscountTotal;
        }

        /// <summary>
        /// Generates a new, unique invoice number based on the current date and a sequence.
        /// </summary>
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
