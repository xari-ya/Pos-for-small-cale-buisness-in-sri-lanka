// -----------------------------------------------------------------------------
// InventoryControl.cs  — Inactive indicator + “Active” column (EXPLAINED)
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace billing_system
{
    public partial class InventoryControl : UserControl
    {
        private DataTable _dt;
        private DataView _view;

        public InventoryControl()
        {
            InitializeComponent();

            // Retitle the last column at runtime (Designer kept intact)
            dataGridViewTextBoxColumn6.HeaderText = "Active";

            WireEvents();
            PrepareGridBinding();
            TryLoadCategories();
            RefreshData();
            ApplyFilters();
        }

        private void PrepareGridBinding()
        {
            dgvProducts.AutoGenerateColumns = false;
            dataGridViewTextBoxColumn1.DataPropertyName = "SKU";
            dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            dataGridViewTextBoxColumn3.DataPropertyName = "Category";
            dataGridViewTextBoxColumn4.DataPropertyName = "Price";
            dataGridViewTextBoxColumn5.DataPropertyName = "Stock";

            // Reuse your “Reorder” column as an “Active” column → binds to IsActive (Yes/No)
            dataGridViewTextBoxColumn6.DataPropertyName = "IsActive";
            dataGridViewTextBoxColumn6.HeaderText = "Active";

            // Style after data binds to mark inactive rows
            dgvProducts.DataBindingComplete += DgvProducts_DataBindingComplete;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
        }

        private void WireEvents()
        {
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cbCategory.SelectedIndexChanged += (s, e) => ApplyFilters();
            numMinStock.ValueChanged += (s, e) => ApplyFilters();

            // ADD
            btnAdd.Click += (s, e) =>
            {
                using var dlg = new ProductDetailForm();
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    using var conn = Database.GetConnection();
                    conn.Open();
                    using var tx = conn.BeginTransaction();

                    long pid = ProductRepository.Insert(conn, tx,
                        dlg.Result.Sku, dlg.Result.Name, dlg.Result.CategoryId,
                        dlg.Result.UnitPrice, 1);

                    if (dlg.Result.OpeningQty > 0)
                    {
                        StockMovementRepository.Insert(conn, tx, pid,
                            "OPENING_STOCK", dlg.Result.OpeningQty, "Initial load", null);
                    }

                    tx.Commit();
                    Console.WriteLine($"[Inventory/Add] OK sku={dlg.Result.Sku} pid={pid}");
                    RefreshData();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Inventory/Add] ERROR: " + ex);
                }
            };

            // EDIT
            btnEdit.Click += (s, e) =>
            {
                if (dgvProducts.CurrentRow == null) return;
                string sku = dgvProducts.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(sku)) return;

                try
                {
                    using var conn = Database.GetConnection();
                    conn.Open();
                    var rec = ProductRepository.GetBySku(conn, sku);
                    if (rec == null) { Console.WriteLine("[Inventory/Edit] Not found: " + sku); return; }

                    using var dlg = new ProductDetailForm(rec.Value.productId, rec.Value.sku, rec.Value.name, rec.Value.categoryId, rec.Value.price);
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    ProductRepository.Update(conn, null, rec.Value.productId,
                        dlg.Result.Sku, dlg.Result.Name, dlg.Result.CategoryId,
                        dlg.Result.UnitPrice, 1);

                    Console.WriteLine($"[Inventory/Edit] OK sku={dlg.Result.Sku} pid={rec.Value.productId}");
                    RefreshData();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Inventory/Edit] ERROR: " + ex);
                }
            };

            // DELETE (Soft → is_active = 0)
            btnDelete.Click += (s, e) =>
            {
                if (dgvProducts.CurrentRow == null) return;
                string sku = dgvProducts.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(sku)) return;

                try
                {
                    using var conn = Database.GetConnection();
                    conn.Open();
                    var rec = ProductRepository.GetBySku(conn, sku);
                    if (rec == null) { Console.WriteLine("[Inventory/Delete] Not found: " + sku); return; }

                    ProductRepository.SoftDelete(conn, null, rec.Value.productId);
                    Console.WriteLine($"[Inventory/Delete] Soft-deleted sku={sku} pid={rec.Value.productId}");
                    RefreshData();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Inventory/Delete] ERROR: " + ex);
                }
            };
        }

        private void TryLoadCategories()
        {
            cbCategory.Items.Clear();
            cbCategory.Items.Add("All");
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                using var cmd = new SQLiteCommand("SELECT name FROM Categories ORDER BY name;", conn);
                using var rd = cmd.ExecuteReader();
                while (rd.Read()) cbCategory.Items.Add(rd.GetString(0));
                Console.WriteLine("[Inventory] Categories loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Inventory] ERROR loading categories: " + ex);
            }
            cbCategory.SelectedIndex = 0;
        }

        private void RefreshData()
        {
            // Note: Replaced "Reorder" with "IsActive" column
            _dt = new DataTable();
            _dt.Columns.Add("SKU", typeof(string));
            _dt.Columns.Add("Name", typeof(string));
            _dt.Columns.Add("Category", typeof(string));
            _dt.Columns.Add("Price", typeof(decimal));
            _dt.Columns.Add("Stock", typeof(int));
            _dt.Columns.Add("IsActive", typeof(string)); // "Yes"/"No" for display

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                VerifyTableExists(conn, "Products");
                VerifyTableExists(conn, "StockMovements");
                VerifyTableExists(conn, "Categories");

                // IMPORTANT: no filter on is_active → we show both active/inactive
                string sql = @"
                    SELECT
                        p.product_id,
                        p.sku,
                        p.name,
                        COALESCE(c.name, 'Uncategorized') AS category,
                        p.unit_price,
                        COALESCE((
                            SELECT SUM(sm.qty_change)
                            FROM StockMovements sm
                            WHERE sm.product_id = p.product_id
                        ), 0) AS stock,
                        COALESCE(p.is_active, 1) AS is_active
                    FROM Products p
                    LEFT JOIN Categories c ON p.category_id = c.category_id
                    ORDER BY p.name;";
                using var cmd = new SQLiteCommand(sql, conn);
                using var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string sku = rd["sku"]?.ToString() ?? "";
                    string name = rd["name"]?.ToString() ?? "";
                    string cat = rd["category"]?.ToString() ?? "Uncategorized";
                    decimal price = Convert.ToDecimal(rd["unit_price"]);
                    int stock = Convert.ToInt32(rd["stock"]);
                    bool isActive = Convert.ToInt32(rd["is_active"]) == 1;

                    _dt.Rows.Add(sku, name, cat, price, stock, isActive ? "Yes" : "No");
                }

                Console.WriteLine($"[Inventory] Loaded {_dt.Rows.Count} products (active + inactive).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Inventory] ERROR while loading products: " + ex);
            }

            _view = new DataView(_dt);
            dgvProducts.DataSource = _view;
            UpdateSummary();
        }

        private void VerifyTableExists(SQLiteConnection conn, string tableName)
        {
            using var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=@t;", conn);
            cmd.Parameters.AddWithValue("@t", tableName);
            var r = cmd.ExecuteScalar();
            if (r == null) throw new InvalidOperationException($"Table '{tableName}' is missing.");
        }

        private void ApplyFilters()
        {
            if (_view == null) return;
            string esc(string s) => s.Replace("'", "''");
            var parts = new List<string>();

            var search = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(search))
                parts.Add($"(SKU LIKE '%{esc(search)}%' OR Name LIKE '%{esc(search)}%')");

            if (cbCategory.SelectedIndex > 0)
            {
                string cat = esc(cbCategory.SelectedItem?.ToString() ?? "");
                parts.Add($"Category = '{cat}'");
            }

            if (numMinStock.Value > 0)
                parts.Add($"Stock >= {numMinStock.Value}");

            _view.RowFilter = parts.Count == 0 ? "" : string.Join(" AND ", parts);
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            var culture = new CultureInfo("en-LK");
            culture.NumberFormat.CurrencySymbol = "Rs.";
            culture.NumberFormat.CurrencyPositivePattern = 2;

            // Count/Value over ACTIVE rows only
            var visible = _view.Cast<DataRowView>().ToList();
            var activeRows = visible.Where(r => string.Equals(r.Row.Field<string>("IsActive"), "Yes", StringComparison.OrdinalIgnoreCase)).ToList();

            int total = activeRows.Count;
            int low = activeRows.Count(r => r.Row.Field<int>("Stock") <= 5);
            int oos = activeRows.Count(r => r.Row.Field<int>("Stock") <= 0);
            decimal value = activeRows.Sum(r => r.Row.Field<decimal>("Price") * r.Row.Field<int>("Stock"));

            lblTotalProducts.Text = $"Total Products: {total}";
            lblLowStock.Text = $"Low Stock: {low}";
            lblOutOfStock.Text = $"Out of Stock: {oos}";
            lblInventoryValue.Text = $"Inventory Value: {value.ToString("C2", culture)}";

            Console.WriteLine($"[Inventory Summary] (Active only) Total={total}, Low={low}, Out={oos}, Value={value}");
        }

        private void DgvProducts_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Grey out inactive rows (no custom painting)
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                var activeCell = row.Cells[dataGridViewTextBoxColumn6.Index]?.Value?.ToString() ?? "Yes";
                bool isActive = activeCell.Equals("Yes", StringComparison.OrdinalIgnoreCase);

                if (!isActive)
                {
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                }
                else
                {
                    // ensure active rows use defaults
                    row.DefaultCellStyle.ForeColor = dgvProducts.DefaultCellStyle.ForeColor;
                    row.DefaultCellStyle.BackColor = dgvProducts.DefaultCellStyle.BackColor;
                }
            }
        }
    }
}
