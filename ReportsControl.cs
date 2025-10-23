// File: ReportsControl.cs  (EXPLAINED VERSION)
// Purpose: Load Sales Summary and Inventory Report from your SQLite DB
// Schema aligned to your Database.Initialize() script:
//   - Invoices(invoice_no PK, issued_at, subtotal, tax_total, discount_total, grand_total, status, created_by -> Users.user_id)
//   - InvoiceItems(invoice_no FK, product_id, qty, line_total, ...)
//   - Users(user_id, full_name)
//   - StockMovements(movement_id, product_id, movement_type, qty_change, ref_invoice_no, created_at)
//   - Products(product_id, sku, name, unit_price)

using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace billing_system
{
    public partial class ReportsControl : UserControl
    {
        // Keep your theme
        private readonly Color PageBg = Color.FromArgb(244, 246, 247);
        private readonly Color Charcoal = Color.FromArgb(33, 37, 41);
        private readonly Color SuccessGreen = Color.FromArgb(46, 204, 113);

        public ReportsControl()
        {
            InitializeComponent();
            ApplyTheme();
            WireEvents();
            PrimeSelectors();

            // Ensure report type defaults
            if (cmbReportType.Items.Count == 0)
            {
                cmbReportType.Items.Add("Sales Summary");
                cmbReportType.Items.Add("Inventory Report");
            }
            if (cmbReportType.SelectedIndex < 0) cmbReportType.SelectedIndex = 0;

            UpdateSummaryPanel(cmbReportType.SelectedItem?.ToString() ?? "Sales Summary");
        }

        // ------------------ Styling ------------------
        private void ApplyTheme()
        {
            root.BackColor = PageBg;
            header.BackColor = PageBg;
            headerTitle.ForeColor = Charcoal;

            controlsPanel.BackColor = Color.White;
            btnGenerate.BackColor = SuccessGreen;
            btnGenerate.ForeColor = Color.White;

            summaryPanel.BackColor = Color.WhiteSmoke;
            var summaryFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            lblTotalSalesTitle.Font = summaryFont;
            lblTotalSalesValue.Font = summaryFont;
            lblTotalSalesValue.ForeColor = SuccessGreen;

            lblStockInTitle.Font = summaryFont;
            lblStockInValue.Font = summaryFont;
            lblStockOutTitle.Font = summaryFont;
            lblStockOutValue.Font = summaryFont;

            reportPanel.BackColor = Color.White;
            lblEmpty.ForeColor = Color.Gray;

            gridReport.BackgroundColor = Color.White;
            gridReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            gridReport.ColumnHeadersDefaultCellStyle.ForeColor = Charcoal;
        }

        // ------------------ Events & Setup ------------------
        private void WireEvents()
        {
            btnGenerate.Click += BtnGenerate_Click;
            cmbReportType.SelectedIndexChanged += OnReportTypeChanged;

            // You can keep month/day enabled; we filter if they’re chosen.
            cmbMonth.Enabled = true;
            cmbDay.Enabled = true;
        }

        private void PrimeSelectors()
        {
            // Months: system locale month names (Jan..Dec)
            if (cmbMonth.Items.Count == 0)
            {
                var monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames
                    .Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
                cmbMonth.Items.AddRange(monthNames);
            }
            int currentMonthIdx = DateTime.Now.Month - 1;
            if (currentMonthIdx >= 0 && currentMonthIdx < cmbMonth.Items.Count)
                cmbMonth.SelectedIndex = currentMonthIdx;
            else if (cmbMonth.Items.Count > 0)
                cmbMonth.SelectedIndex = 0;

            // Days: “Any” + 01..31
            if (cmbDay.Items.Count == 0)
            {
                cmbDay.Items.Add("— Any Date —");
                for (int d = 1; d <= 31; d++) cmbDay.Items.Add(d.ToString("00"));
            }
            if (cmbDay.SelectedIndex < 0) cmbDay.SelectedIndex = 0;

            lblEmpty.Visible = true;
            gridReport.DataSource = null;
        }

        private void OnReportTypeChanged(object? sender, EventArgs e)
        {
            string type = cmbReportType.SelectedItem?.ToString() ?? "Sales Summary";
            UpdateSummaryPanel(type);
            gridReport.DataSource = null;
            lblEmpty.Text = "Select filters and click Generate.";
            lblEmpty.Visible = true;
        }

        private void UpdateSummaryPanel(string reportType)
        {
            bool isInventory = reportType.Equals("Inventory Report", StringComparison.OrdinalIgnoreCase);

            lblTotalSalesTitle.Visible = !isInventory;
            lblTotalSalesValue.Visible = !isInventory;

            lblStockInTitle.Visible = isInventory;
            lblStockInValue.Visible = isInventory;
            lblStockOutTitle.Visible = isInventory;
            lblStockOutValue.Visible = isInventory;

            // Reset values
            lblTotalSalesValue.Text = "LKR 0.00";
            lblStockInValue.Text = "0";
            lblStockOutValue.Text = "0";
        }

        // ------------------ Generate ------------------
        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            string type = cmbReportType.SelectedItem?.ToString() ?? "Sales Summary";

            try
            {
                if (type == "Inventory Report")
                    LoadInventoryFromDb();
                else
                    LoadSalesFromDb();

                lblEmpty.Visible = (gridReport.DataSource == null || gridReport.Rows.Count == 0);
            }
            catch (Exception ex)
            {
                gridReport.DataSource = null;
                lblEmpty.Visible = true;
                // Friendly message (keeps designer safe)
                lblEmpty.Text = "Could not load data.\n" +
                                "• Ensure DB file exists and schema is initialized.\n" +
                                "• Needed tables: Users, Products, Invoices, InvoiceItems, StockMovements.\n" +
                                "• Error: " + ex.Message;
            }
        }

        // ------------------ SALES (from DB) ------------------
        private void LoadSalesFromDb()
        {
            // Filters from pickers
            string mm = GetSelectedMonthTwoDigits();      // 01..12
            string? dd = GetSelectedDayTwoDigitsOrNull(); // null = Any day

            // IMPORTANT: Your schema uses invoice_no as PK and issued_at as date.
            // Items: SUM of InvoiceItems.qty per invoice_no
            // Cashier: Users.full_name via Invoices.created_by
            string sql = @"
SELECT
    i.invoice_no                                        AS [Bill ID],
    IFNULL((SELECT SUM(ii.qty) FROM InvoiceItems ii WHERE ii.invoice_no = i.invoice_no), 0) AS [Items],
    i.issued_at                                         AS [Paid Date],
    i.subtotal                                          AS [Subtotal],
    i.discount_total                                    AS [Discount],
    i.tax_total                                         AS [Tax],
    i.grand_total                                       AS [Total],
    u.full_name                                         AS [Cashier]
FROM Invoices i
LEFT JOIN Users u ON u.user_id = i.created_by
WHERE i.status = 'PAID'
  AND strftime('%m', i.issued_at) = @mm
" + (dd != null ? "  AND strftime('%d', i.issued_at) = @dd\n" : "") + @"
ORDER BY i.issued_at ASC;";

            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mm", mm);
            if (dd != null) cmd.Parameters.AddWithValue("@dd", dd);

            var dt = new DataTable();
            conn.Open();
            using (var rd = cmd.ExecuteReader()) dt.Load(rd);

            gridReport.DataSource = dt;
            MakeColumnsSortable();
            ApplyMoneyFormats(new[] { "Subtotal", "Discount", "Tax", "Total" });

            // Summary: Total Sales
            double totalSales = 0.0;
            foreach (DataRow r in dt.Rows)
            {
                if (r["Total"] != DBNull.Value)
                    totalSales += Convert.ToDouble(r["Total"]);
            }
            lblTotalSalesValue.Text = "LKR " + totalSales.ToString("#,##0.00", CultureInfo.InvariantCulture);
        }

        // ------------------ INVENTORY (from DB) ------------------
        private void LoadInventoryFromDb()
        {
            string mm = GetSelectedMonthTwoDigits();
            string? dd = GetSelectedDayTwoDigitsOrNull();

            string sql = @"
SELECT
    IFNULL(sm.movement_id, '(no id)')                 AS [Txn ID],
    sm.created_at                                     AS [Date],
    IFNULL(p.sku, '(unknown)')                        AS [SKU],
    IFNULL(p.name, '(unknown product)')               AS [Item],
    COALESCE(sm.qty_change, 0)                        AS [Change],
    sm.movement_type                                  AS [Type],
    COALESCE(p.unit_price, 0)                         AS [UnitCost],
    (COALESCE(p.unit_price, 0) * COALESCE(sm.qty_change, 0)) AS [Amount]
FROM StockMovements sm
LEFT JOIN Products p ON p.product_id = sm.product_id
WHERE strftime('%m', sm.created_at) = @mm
" + (dd != null ? "  AND strftime('%d', sm.created_at) = @dd\n" : "") + @"
ORDER BY sm.created_at ASC;";

            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mm", mm);
            if (dd != null) cmd.Parameters.AddWithValue("@dd", dd);

            var dt = new DataTable();
            conn.Open();
            using (var rd = cmd.ExecuteReader()) dt.Load(rd);

            gridReport.DataSource = dt;
            MakeColumnsSortable();
            ApplyMoneyFormats(new[] { "UnitCost", "Amount" });

            int stockIn = 0, stockOut = 0;
            foreach (DataRow r in dt.Rows)
            {
                if (r["Change"] == DBNull.Value) continue;
                int ch = Convert.ToInt32(r["Change"]);
                if (ch > 0) stockIn += ch; else stockOut += ch;
            }
            lblStockInValue.Text = stockIn.ToString();
            lblStockOutValue.Text = Math.Abs(stockOut).ToString();
        }


        // ------------------ Helpers ------------------
        private void ApplyMoneyFormats(string[] columnNames)
        {
            foreach (var name in columnNames)
            {
                if (gridReport.Columns.Contains(name))
                    gridReport.Columns[name].DefaultCellStyle.Format = "#,##0.00";
            }

            // Prefix with "LKR " via CellFormatting so sorting still works numerically
            gridReport.CellFormatting -= GridReport_CellFormatting;
            gridReport.CellFormatting += GridReport_CellFormatting;
        }

        private void GridReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = gridReport.Columns[e.ColumnIndex].Name;
            if (e.Value == null || e.Value == DBNull.Value) return;

            if (colName is "Subtotal" or "Discount" or "Tax" or "Total" or "UnitCost" or "Amount")
            {
                if (e.Value is IFormattable f)
                {
                    e.Value = "LKR " + f.ToString("#,##0.00", CultureInfo.InvariantCulture);
                    e.FormattingApplied = true;
                }
            }
        }

        private void MakeColumnsSortable()
        {
            foreach (DataGridViewColumn col in gridReport.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private string GetSelectedMonthTwoDigits()
        {
            int month = (cmbMonth?.SelectedIndex ?? (DateTime.Now.Month - 1)) + 1;
            if (month < 1 || month > 12) month = DateTime.Now.Month;
            return month.ToString("00");
        }

        private string? GetSelectedDayTwoDigitsOrNull()
        {
            if (cmbDay?.SelectedIndex == 0) return null; // Any
            if (cmbDay?.SelectedItem is string s && int.TryParse(s, out int d) && d >= 1 && d <= 31)
                return d.ToString("00");
            return null;
        }
    }
}
