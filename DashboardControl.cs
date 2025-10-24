using System;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the dashboard user control, which serves as the main landing screen for administrators.
    /// </summary>
    /// <remarks>
    /// This control displays key performance indicators (KPIs) such as today's sales, total inventory, and low stock alerts.
    /// It also shows a grid of recent transactions. The data is fetched directly from the database upon loading.
    /// </remarks>
    public partial class DashboardControl : UserControl
    {
        private const int LowStockThreshold = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardControl"/> class.
        /// </summary>
        public DashboardControl()
        {
            InitializeComponent();
            this.Load += DashboardControl_Load;
        }

        /// <summary>
        /// Handles the Load event for the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// When the control loads, it sets a welcome message for the current user, loads the KPIs,
        /// and populates the grid with recent invoices. It also performs some diagnostic logging.
        /// </remarks>
        private void DashboardControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            lblWelcomeUser.Text = $"Welcome, {AppSession.CurrentUser?.FullName ?? "Owner"}";
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

        /// <summary>
        /// Loads and calculates the Key Performance Indicators (KPIs) from the database.
        /// </summary>
        /// <remarks>
        /// This method executes several SQL queries to calculate:
        /// 1. Today's total sales for paid invoices.
        /// 2. The total number of items in stock based on stock movements.
        /// 3. The number of products with low stock levels (below <see cref="LowStockThreshold"/>).
        /// It then calls <see cref="CreateKpiPanel"/> to update the UI.
        /// </remarks>
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
                    const string sqlInv = "SELECT COALESCE(SUM(qty_change), 0) FROM StockMovements;";
                    Log("KPI#2 SQL (Total Inventory): " + sqlInv);
                    using (var cmd = new SQLiteCommand(sqlInv, conn))
                    {
                        var v = cmd.ExecuteScalar();
                        Log("KPI#2 raw: " + (v?.ToString() ?? "NULL"));
                        if (v != null && v != DBNull.Value) totalInvRaw = Convert.ToDouble(v);
                        totalInventory = Convert.ToInt32(Math.Round(totalInvRaw));
                    }
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
            var us = new CultureInfo("en-US");
            TryApplySystemIcons();
            CreateKpiPanel(panel: panelSales, icon: iconSales, title: lblSalesTitle, value: lblSalesValue, titleText: "Today's Sales", valueText: todaysSales.ToString("C", us), valueColor: Color.FromArgb(0, 123, 255));
            CreateKpiPanel(panel: panelInventory, icon: iconInventory, title: lblInventoryTitle, value: lblInventoryValue, titleText: "Items in Stock", valueText: totalInventory.ToString("N0"), valueColor: Color.FromArgb(40, 167, 69));
            CreateKpiPanel(panel: panelAttention, icon: iconAttention, title: lblAttentionTitle, value: lblAttentionValue, titleText: "Low Stock Alerts", valueText: attentionNeeded.ToString(), valueColor: attentionNeeded > 0 ? Color.FromArgb(220, 53, 69) : Color.Gray);
            Log($"KPIs displayed => TodaySales={todaysSales:C}, TotalInventory={totalInventory}, Attention={attentionNeeded}");
            DebugKpiLayout();
        }

        /// <summary>
        /// Loads the most recent invoices into the data grid view.
        /// </summary>
        /// <param name="maxRows">The maximum number of invoices to load.</param>
        /// <remarks>
        /// This method retrieves a summary of recent invoices, including the total number of items and payment method,
        /// and populates the <c>dgvRecentTransactions</c> grid.
        /// </remarks>
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

        /// <summary>
        /// A helper method to create and configure a KPI panel.
        /// </summary>
        /// <param name="panel">The main panel for the KPI.</param>
        /// <param name="icon">The PictureBox for the KPI icon.</param>
        /// <param name="title">The Label for the KPI title.</param>
        /// <param name="value">The Label for the KPI value.</param>
        /// <param name="titleText">The text to display as the title.</param>
        /// <param name="valueText">The text to display as the value.</param>
        /// <param name="valueColor">The color to apply to the value text.</param>
        /// <remarks>
        /// This method, likely located in the Designer.cs file, populates the text and styles for a single KPI display panel.
        /// </remarks>

        /// <summary>
        /// Attempts to apply system icons to the KPI panels.
        /// </summary>
        /// <remarks>
        /// This method sets the images for the KPI icons using standard system icons. It is wrapped in a try-catch block
        /// to prevent crashes in the designer.
        /// </remarks>
        private void TryApplySystemIcons()
        {
            try
            {
                iconSales.Image = SystemIcons.Information.ToBitmap();
                iconInventory.Image = SystemIcons.Application.ToBitmap();
                iconAttention.Image = SystemIcons.Warning.ToBitmap();
            }
            catch { }
        }

        /// <summary>
        /// Logs a message to the console with a dashboard-specific prefix.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        private void Log(string msg)
        {
            try { Console.WriteLine("[Dashboard] " + msg); } catch { }
        }

        /// <summary>
        /// Logs the current layout state of the KPI panel for debugging.
        /// </summary>
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

        /// <summary>
        /// Dumps the row counts of specified tables to the log for debugging.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tables">An array of table names to query.</param>
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
