using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the user control for generating and viewing reports.
    /// </summary>
    /// <remarks>
    /// This control allows users to generate two types of reports: a Sales Summary and an Inventory Report.
    /// Users can filter the data by month and day. The results are displayed in a data grid, and a summary
    /// panel shows key totals for the selected report.
    /// </remarks>
    public partial class ReportsControl : UserControl
    {
        private readonly Color PageBg = Color.FromArgb(244, 246, 247);
        private readonly Color Charcoal = Color.FromArgb(33, 37, 41);
        private readonly Color SuccessGreen = Color.FromArgb(46, 204, 113);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsControl"/> class.
        /// </summary>
        public ReportsControl()
        {
            InitializeComponent();
            ApplyTheme();
            WireEvents();
            PrimeSelectors();
            if (cmbReportType.Items.Count == 0)
            {
                cmbReportType.Items.Add("Sales Summary");
                cmbReportType.Items.Add("Inventory Report");
            }
            if (cmbReportType.SelectedIndex < 0) cmbReportType.SelectedIndex = 0;
            UpdateSummaryPanel(cmbReportType.SelectedItem?.ToString() ?? "Sales Summary");
        }

        /// <summary>
        /// Applies a consistent theme to the control's UI elements.
        /// </summary>
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

        /// <summary>
        /// Wires up event handlers for the interactive controls.
        /// </summary>
        private void WireEvents()
        {
            btnGenerate.Click += BtnGenerate_Click;
            cmbReportType.SelectedIndexChanged += OnReportTypeChanged;
            cmbMonth.Enabled = true;
            cmbDay.Enabled = true;
        }

        /// <summary>
        /// Initializes the selection controls (combo boxes) with default values.
        /// </summary>
        private void PrimeSelectors()
        {
            if (cmbMonth.Items.Count == 0)
            {
                var monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames.Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
                cmbMonth.Items.AddRange(monthNames);
            }
            int currentMonthIdx = DateTime.Now.Month - 1;
            if (currentMonthIdx >= 0 && currentMonthIdx < cmbMonth.Items.Count)
                cmbMonth.SelectedIndex = currentMonthIdx;
            else if (cmbMonth.Items.Count > 0)
                cmbMonth.SelectedIndex = 0;
            if (cmbDay.Items.Count == 0)
            {
                cmbDay.Items.Add("— Any Date —");
                for (int d = 1; d <= 31; d++) cmbDay.Items.Add(d.ToString("00"));
            }
            if (cmbDay.SelectedIndex < 0) cmbDay.SelectedIndex = 0;
            lblEmpty.Visible = true;
            gridReport.DataSource = null;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event for the report type combo box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method updates the visibility of the summary panels based on the selected report type and clears the grid.
        /// </remarks>
        private void OnReportTypeChanged(object? sender, EventArgs e)
        {
            string type = cmbReportType.SelectedItem?.ToString() ?? "Sales Summary";
            UpdateSummaryPanel(type);
            gridReport.DataSource = null;
            lblEmpty.Text = "Select filters and click Generate.";
            lblEmpty.Visible = true;
        }

        /// <summary>
        /// Updates the visibility of the summary panels based on the selected report type.
        /// </summary>
        /// <param name="reportType">The name of the selected report type.</param>
        private void UpdateSummaryPanel(string reportType)
        {
            bool isInventory = reportType.Equals("Inventory Report", StringComparison.OrdinalIgnoreCase);
            lblTotalSalesTitle.Visible = !isInventory;
            lblTotalSalesValue.Visible = !isInventory;
            lblStockInTitle.Visible = isInventory;
            lblStockInValue.Visible = isInventory;
            lblStockOutTitle.Visible = isInventory;
            lblStockOutValue.Visible = isInventory;
            lblTotalSalesValue.Text = "LKR 0.00";
            lblStockInValue.Text = "0";
            lblStockOutValue.Text = "0";
        }

        /// <summary>
        /// Handles the Click event for the "Generate" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method triggers the appropriate data loading method based on the selected report type.
        /// </remarks>
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
                lblEmpty.Text = "Could not load data.\n" + "• Ensure DB file exists and schema is initialized.\n" + "• Needed tables: Users, Products, Invoices, InvoiceItems, StockMovements.\n" + "• Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Loads the sales summary report data from the database.
        /// </summary>
        /// <remarks>
        /// This method constructs and executes a SQL query to retrieve details of paid invoices for the selected period.
        /// The results are loaded into a <see cref="DataTable"/> and displayed in the grid. It also calculates and displays the total sales.
        /// </remarks>
        private void LoadSalesFromDb()
        {
            string mm = GetSelectedMonthTwoDigits();
            string? dd = GetSelectedDayTwoDigitsOrNull();
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
            double totalSales = 0.0;
            foreach (DataRow r in dt.Rows)
            {
                if (r["Total"] != DBNull.Value)
                    totalSales += Convert.ToDouble(r["Total"]);
            }
            lblTotalSalesValue.Text = "LKR " + totalSales.ToString("#,##0.00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Loads the inventory report data from the database.
        /// </summary>
        /// <remarks>
        /// This method constructs and executes a SQL query to retrieve all stock movements for the selected period.
        /// The results are displayed in the grid, and the summary panel is updated with the total stock in and stock out quantities.
        /// </remarks>
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

        /// <summary>
        /// Applies a currency format to the specified columns in the data grid.
        /// </summary>
        /// <param name="columnNames">An array of column names to format.</param>
        private void ApplyMoneyFormats(string[] columnNames)
        {
            foreach (var name in columnNames)
            {
                if (gridReport.Columns.Contains(name))
                    gridReport.Columns[name].DefaultCellStyle.Format = "#,##0.00";
            }
            gridReport.CellFormatting -= GridReport_CellFormatting;
            gridReport.CellFormatting += GridReport_CellFormatting;
        }

        /// <summary>
        /// Handles the CellFormatting event to add a currency prefix to monetary values.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This approach allows the underlying cell value to remain numeric for correct sorting.
        /// </remarks>
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

        /// <summary>
        /// Enables automatic sorting for all columns in the data grid.
        /// </summary>
        private void MakeColumnsSortable()
        {
            foreach (DataGridViewColumn col in gridReport.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        /// <summary>
        /// Gets the selected month from the combo box as a two-digit string.
        /// </summary>
        /// <returns>A string representing the selected month (e.g., "01" for January).</returns>
        private string GetSelectedMonthTwoDigits()
        {
            int month = (cmbMonth?.SelectedIndex ?? (DateTime.Now.Month - 1)) + 1;
            if (month < 1 || month > 12) month = DateTime.Now.Month;
            return month.ToString("00");
        }

        /// <summary>
        /// Gets the selected day from the combo box as a two-digit string.
        /// </summary>
        /// <returns>A string representing the selected day, or <c>null</c> if "Any" is selected.</returns>
        private string? GetSelectedDayTwoDigitsOrNull()
        {
            if (cmbDay?.SelectedIndex == 0) return null;
            if (cmbDay?.SelectedItem is string s && int.TryParse(s, out int d) && d >= 1 && d <= 31)
                return d.ToString("00");
            return null;
        }
    }
}
