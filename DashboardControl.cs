using System;
using System.Data.SQLite;
using System.Drawing; // Added for Color
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
    
    
namespace billing_system
{
    public partial class DashboardControl : UserControl
    {
        private const int LowStockThreshold = 5;

        public DashboardControl()
        {
            InitializeComponent();
            this.Load += DashboardControl_Load;
        }

        private void DashboardControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            lblWelcomeUser.Text = $"Welcome, {AppSession.CurrentUser?.FullName ?? "Owner"}";

            // This is no longer needed here, as CreateKpiPanel handles the icons
            // TryApplySystemIcons(); 

            // --- DB path + row counts for quick sanity check ---
            try
            {
                using var conn = Database.GetConnection();
                Log("DB Path: " + conn.DataSource);
                conn.Open();
                DumpTableCounts(conn, "Invoices", "InvoiceItems", "Payments", "StockMovements", "Products", "Users");
            }
            catch (Exception ex)
            {
                Log("DB open error: " + ex.Message);
            }

            // --- Ensure dock order so KPI row is never hidden by the grid ---
            try
            {
                if (headerPanel != null) Controls.SetChildIndex(headerPanel, 0);
                if (kpiTableLayout != null) Controls.SetChildIndex(kpiTableLayout, 1);
                if (lblRecentTransactions != null) Controls.SetChildIndex(lblRecentTransactions, 2);
                if (dgvRecentTransactions != null) Controls.SetChildIndex(dgvRecentTransactions, 3);

                headerPanel?.BringToFront();
                kpiTableLayout?.BringToFront();
                lblRecentTransactions?.BringToFront();
                dgvRecentTransactions?.BringToFront();

                if (kpiTableLayout != null)
                {
                    kpiTableLayout.Visible = true;
                    if (kpiTableLayout.Height < 160) kpiTableLayout.Height = 200;
                }
            }
            catch (Exception ex)
            {
                Log("Dock order fix error: " + ex.Message);
            }

            DebugKpiLayout();

            LoadKpis();
            LoadRecentInvoices(100);
        }

        // ---------------------------------------------------------------------
        // KPI loading (with very verbose logging)
        // ---------------------------------------------------------------------
        private void LoadKpis()
        {
            decimal todaysSales = 0m;
            double totalInvRaw = 0.0;
            int totalInventory = 0;
            int attentionNeeded = 0;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    // 1) Today's Sales (PAID + issued today)
                    const string sqlToday = @"
                        SELECT COALESCE(SUM(grand_total), 0)
                        FROM Invoices
                        WHERE status = 'PAID'
                          AND date(issued_at) = date('now','localtime');";
                    Log("KPI#1 SQL (Today Sales): " + sqlToday);
                    using (var cmd = new SQLiteCommand(sqlToday, conn))
                    {
                        var v = cmd.ExecuteScalar();
                        Log("KPI#1 raw: " + (v?.ToString() ?? "NULL"));
                        if (v != null && v != DBNull.Value) todaysSales = Convert.ToDecimal(v);
                    }

                    // Optional fallback
                    if (todaysSales == 0m)
                    {
                        string sqlLatestDate = "SELECT MAX(date(issued_at)) FROM Invoices WHERE status='PAID';";
                        using var getDate = new SQLiteCommand(sqlLatestDate, conn);
                        var latestDateObj = getDate.ExecuteScalar();
                        var latestDate = latestDateObj?.ToString();
                        Log("Latest PAID invoice date: " + (latestDate ?? "NULL"));

                        if (!string.IsNullOrEmpty(latestDate))
                        {
                            string sqlLatestSum = @"
                                SELECT COALESCE(SUM(grand_total), 0)
                                FROM Invoices
                                WHERE status = 'PAID' AND date(issued_at) = @d;";
                            using var sumLatest = new SQLiteCommand(sqlLatestSum, conn);
                            sumLatest.Parameters.AddWithValue("@d", latestDate);
                            var v2 = sumLatest.ExecuteScalar();
                            Log("KPI#1 fallback raw (latest date sum): " + (v2?.ToString() ?? "NULL"));
                            if (v2 != null && v2 != DBNull.Value)
                            {
                                var fallback = Convert.ToDecimal(v2);
                                if (fallback > 0m) todaysSales = fallback;
                            }
                        }
                    }

                    // 2) Total Inventory (net sum of movements)
                    const string sqlInv = "SELECT COALESCE(SUM(qty_change), 0) FROM StockMovements;";
                    Log("KPI#2 SQL (Total Inventory): " + sqlInv);
                    using (var cmd = new SQLiteCommand(sqlInv, conn))
                    {
                        var v = cmd.ExecuteScalar();
                        Log("KPI#2 raw: " + (v?.ToString() ?? "NULL"));
                        if (v != null && v != DBNull.Value) totalInvRaw = Convert.ToDouble(v);
                        totalInventory = Convert.ToInt32(Math.Round(totalInvRaw));
                    }

                    // 3) Attention Needed (qty <= threshold)
                    const string sqlAtt = @"
                        SELECT product_id, COALESCE(SUM(qty_change),0) AS qty
                        FROM StockMovements
                        GROUP BY product_id
                        HAVING qty <= @th;";
                    Log("KPI#3 SQL (Attention Needed): " + sqlAtt);
                    using (var cmd = new SQLiteCommand(sqlAtt, conn))
                    {
                        cmd.Parameters.AddWithValue("@th", LowStockThreshold);
                        using var rd = cmd.ExecuteReader();
                        attentionNeeded = 0;
                        while (rd.Read())
                        {
                            attentionNeeded++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log("LoadKpis ERROR: " + ex.Message);
            }

            // =================================================================
            // === FIX: Call the helper method to build the UI for each KPI ===
            // =================================================================
            var us = new CultureInfo("en-US");

            // Apply system icons *before* creating the panels
            TryApplySystemIcons();

            // Create the "Today's Sales" KPI Panel
            CreateKpiPanel(
                panel: panelSales,
                icon: iconSales,
                title: lblSalesTitle,
                value: lblSalesValue,
                titleText: "Today's Sales",
                valueText: todaysSales.ToString("C", us),
                valueColor: Color.FromArgb(0, 123, 255)
            );

            // Create the "Items in Stock" KPI Panel
            CreateKpiPanel(
                panel: panelInventory,
                icon: iconInventory,
                title: lblInventoryTitle,
                value: lblInventoryValue,
                titleText: "Items in Stock",
                valueText: totalInventory.ToString("N0"), // Format with commas
                valueColor: Color.FromArgb(40, 167, 69)
            );

            // Create the "Low Stock Alerts" KPI Panel
            CreateKpiPanel(
                panel: panelAttention,
                icon: iconAttention,
                title: lblAttentionTitle,
                value: lblAttentionValue,
                titleText: "Low Stock Alerts",
                valueText: attentionNeeded.ToString(),
                valueColor: attentionNeeded > 0 ? Color.FromArgb(220, 53, 69) : Color.Gray
            );

            Log($"KPIs displayed => TodaySales={todaysSales:C}, TotalInventory={totalInventory}, Attention={attentionNeeded}");

            // Final check of layout after updating labels
            DebugKpiLayout();
        }

        // ---------------------------------------------------------------------
        // Grid: last N invoices (newest first) — Items = SUM(qty)
        // ---------------------------------------------------------------------
        private void LoadRecentInvoices(int maxRows)
        {
            dgvRecentTransactions.Rows.Clear();
            dgvRecentTransactions.AutoGenerateColumns = false;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            i.invoice_no AS InvoiceNo,
                            i.issued_at  AS IssuedAt,
                            (SELECT COALESCE(SUM(x.qty),0)
                               FROM InvoiceItems x
                              WHERE x.invoice_no = i.invoice_no) AS Items,
                            i.grand_total AS GrandTotal,
                            (SELECT p.method FROM Payments p
                              WHERE p.invoice_no = i.invoice_no
                              ORDER BY p.received_at DESC, p.rowid DESC LIMIT 1) AS Method
                        FROM Invoices i
                        ORDER BY i.issued_at DESC, i.invoice_no DESC
                        LIMIT @limit;";

                    Log("Grid SQL: " + sql);

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@limit", maxRows);
                        using (var rd = cmd.ExecuteReader())
                        {
                            var us = new CultureInfo("en-US");
                            int rowCounter = 0;

                            while (rd.Read())
                            {
                                string inv = rd["InvoiceNo"]?.ToString() ?? "";
                                string dtStr = rd["IssuedAt"]?.ToString() ?? "";
                                // Attempt to parse and format the date for consistency
                                string formattedDate = DateTime.TryParse(dtStr, out var dt) ? dt.ToString("g") : dtStr;

                                int items = rd["Items"] != DBNull.Value ? Convert.ToInt32(rd["Items"]) : 0;
                                decimal tot = rd["GrandTotal"] != DBNull.Value ? Convert.ToDecimal(rd["GrandTotal"]) : 0m;
                                string meth = rd["Method"]?.ToString() ?? "—";

                                dgvRecentTransactions.Rows.Add(inv, formattedDate, items, tot.ToString("C", us), meth);
                                rowCounter++;
                            }

                            Log($"[Grid] Loaded {rowCounter} invoice rows.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log("[Dashboard.LoadRecentInvoices] " + ex.Message);
            }
        }

        // ---------------------------------------------------------------------
        // Helpers: icons + DB snapshot + logging utilities
        // ---------------------------------------------------------------------
        private void TryApplySystemIcons()
        {
            try
            {
                // You can replace these with your own icons from Properties.Resources
                // e.g., iconSales.Image = Properties.Resources.my_sales_icon;
                iconSales.Image = SystemIcons.Information.ToBitmap();
                iconInventory.Image = SystemIcons.Application.ToBitmap();
                iconAttention.Image = SystemIcons.Warning.ToBitmap();
            }
            catch { /* ignore if designer crashes */ }
        }

        // ---------- DEBUG/LOG HELPERS ----------
        private void Log(string msg)
        {
            try { Console.WriteLine("[Dashboard] " + msg); } catch { }
        }

        private void DebugKpiLayout()
        {
            try
            {
                Log($"KPI Layout: {(kpiTableLayout?.Visible == true ? "Visible" : "Hidden")}, " +
                    $"Height={kpiTableLayout?.Height}, Dock={kpiTableLayout?.Dock}");
                if (panelSales != null)
                    Log($"panelSales Size={panelSales.Size}, Visible={panelSales.Visible}");
            }
            catch { }
        }

        private void DumpTableCounts(SQLiteConnection conn, params string[] tables)
        {
            foreach (var t in tables)
            {
                try
                {
                    using var c = new SQLiteCommand($"SELECT COUNT(1) FROM [{t}];", conn);
                    var v = c.ExecuteScalar();
                    long n = (v != null && v != DBNull.Value) ? Convert.ToInt64(v) : 0L;
                    Log($"Table '{t}': {n} rows");
                }
                catch (Exception ex)
                {
                    Log($"Table '{t}': ERROR -> {ex.Message}");
                }
            }
        }
    }
}