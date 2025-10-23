using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace billing_system
{
    partial class ReportsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        // --- All UI element definitions are here ---
        private TableLayoutPanel root;
        private Panel header;
        private Label headerTitle;
        private Panel controlsPanel;
        private FlowLayoutPanel controlsRow;
        private Label lblReportType;
        private ComboBox cmbReportType;
        private Label lblMonth;
        private ComboBox cmbMonth;
        private Label lblDay;
        private ComboBox cmbDay;
        private Button btnGenerate;

        // NEW: Summary Panel controls
        private Panel summaryPanel;
        private FlowLayoutPanel summaryFlowLayout;
        private Label lblTotalSalesTitle;
        private Label lblTotalSalesValue;
        private Label lblStockInTitle;
        private Label lblStockInValue;
        private Label lblStockOutTitle;
        private Label lblStockOutValue;

        private Panel reportPanel;
        private Label lblEmpty;
        private DataGridView gridReport;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.root = new System.Windows.Forms.TableLayoutPanel();
            this.header = new System.Windows.Forms.Panel();
            this.headerTitle = new System.Windows.Forms.Label();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.controlsRow = new System.Windows.Forms.FlowLayoutPanel();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.lblMonth = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.lblDay = new System.Windows.Forms.Label();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();

            // NEW: Summary Panel definitions
            this.summaryPanel = new System.Windows.Forms.Panel();
            this.summaryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotalSalesTitle = new System.Windows.Forms.Label();
            this.lblTotalSalesValue = new System.Windows.Forms.Label();
            this.lblStockInTitle = new System.Windows.Forms.Label();
            this.lblStockInValue = new System.Windows.Forms.Label();
            this.lblStockOutTitle = new System.Windows.Forms.Label();
            this.lblStockOutValue = new System.Windows.Forms.Label();

            this.reportPanel = new System.Windows.Forms.Panel();
            this.lblEmpty = new System.Windows.Forms.Label();
            this.gridReport = new System.Windows.Forms.DataGridView();

            // Suspend layout for performance
            this.root.SuspendLayout();
            this.header.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.controlsRow.SuspendLayout();

            // NEW: Suspend layout for new panel
            this.summaryPanel.SuspendLayout();
            this.summaryFlowLayout.SuspendLayout();

            this.reportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).BeginInit();
            this.SuspendLayout();

            // 
            // root
            // 
            this.root.ColumnCount = 1;
            this.root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.root.Controls.Add(this.header, 0, 0);
            this.root.Controls.Add(this.controlsPanel, 0, 1);

            // NEW: Add summaryPanel to Row 2
            this.root.Controls.Add(this.summaryPanel, 0, 2);
            // MODIFIED: Move reportPanel to Row 3
            this.root.Controls.Add(this.reportPanel, 0, 3);

            this.root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.root.Location = new System.Drawing.Point(0, 0);
            this.root.Name = "root";

            // MODIFIED: Changed RowCount to 4
            this.root.RowCount = 4;
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // NEW: Add RowStyle for summaryPanel
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.root.Size = new System.Drawing.Size(900, 600);
            this.root.TabIndex = 0;

            // ... (header and controlsPanel are unchanged) ...

            // 
            // header
            // 
            this.header.Controls.Add(this.headerTitle);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(16);
            this.header.Size = new System.Drawing.Size(900, 70);
            this.header.TabIndex = 0;
            // 
            // headerTitle
            // 
            this.headerTitle.AutoSize = true;
            this.headerTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.headerTitle.Location = new System.Drawing.Point(16, 18);
            this.headerTitle.Name = "headerTitle";
            this.headerTitle.Size = new System.Drawing.Size(244, 32);
            this.headerTitle.TabIndex = 0;
            this.headerTitle.Text = "Reports & Analytics";
            // 
            // controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.controlsRow);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlsPanel.Location = new System.Drawing.Point(0, 70);
            this.controlsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.controlsPanel.Size = new System.Drawing.Size(900, 76);
            this.controlsPanel.TabIndex = 1;
            // 
            // controlsRow
            // 
            this.controlsRow.Controls.Add(this.lblReportType);
            this.controlsRow.Controls.Add(this.cmbReportType);
            this.controlsRow.Controls.Add(this.lblMonth);
            this.controlsRow.Controls.Add(this.cmbMonth);
            this.controlsRow.Controls.Add(this.lblDay);
            this.controlsRow.Controls.Add(this.cmbDay);
            this.controlsRow.Controls.Add(this.btnGenerate);
            this.controlsRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlsRow.Location = new System.Drawing.Point(16, 8);
            this.controlsRow.Margin = new System.Windows.Forms.Padding(0);
            this.controlsRow.Name = "controlsRow";
            this.controlsRow.Size = new System.Drawing.Size(868, 60);
            this.controlsRow.TabIndex = 0;
            this.controlsRow.WrapContents = false;
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Location = new System.Drawing.Point(0, 10);
            this.lblReportType.Margin = new System.Windows.Forms.Padding(0, 10, 6, 0);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(70, 15);
            this.lblReportType.TabIndex = 0;
            this.lblReportType.Text = "Report Type";
            // 
            // cmbReportType
            // 
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.AddRange(new object[] {
            "Sales Summary",
            "Inventory Report"});
            this.cmbReportType.Location = new System.Drawing.Point(76, 4);
            this.cmbReportType.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(200, 23);
            this.cmbReportType.TabIndex = 1;
            this.cmbReportType.SelectedIndex = 0; // Set default
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Location = new System.Drawing.Point(296, 10);
            this.lblMonth.Margin = new System.Windows.Forms.Padding(8, 10, 6, 0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(43, 15);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Month";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(345, 4);
            this.cmbMonth.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(140, 23);
            this.cmbMonth.TabIndex = 3;
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(505, 10);
            this.lblDay.Margin = new System.Windows.Forms.Padding(8, 10, 6, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(89, 15);
            this.lblDay.TabIndex = 4;
            this.lblDay.Text = "Date (optional)";
            // 
            // cmbDay
            // 
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(600, 4);
            this.cmbDay.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(140, 23);
            this.cmbDay.TabIndex = 5;
            // 
            // btnGenerate
            // 
            this.btnGenerate.AutoSize = true;
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Location = new System.Drawing.Point(760, 2);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(8, 2, 0, 0);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 27);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;

            // 
            // NEW: summaryPanel
            // 
            this.summaryPanel.Controls.Add(this.summaryFlowLayout);
            this.summaryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryPanel.Location = new System.Drawing.Point(0, 146);
            this.summaryPanel.Margin = new System.Windows.Forms.Padding(0);
            this.summaryPanel.Name = "summaryPanel";
            this.summaryPanel.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.summaryPanel.Size = new System.Drawing.Size(900, 50);
            this.summaryPanel.TabIndex = 2;
            // 
            // NEW: summaryFlowLayout
            // 
            this.summaryFlowLayout.Controls.Add(this.lblTotalSalesTitle);
            this.summaryFlowLayout.Controls.Add(this.lblTotalSalesValue);
            this.summaryFlowLayout.Controls.Add(this.lblStockInTitle);
            this.summaryFlowLayout.Controls.Add(this.lblStockInValue);
            this.summaryFlowLayout.Controls.Add(this.lblStockOutTitle);
            this.summaryFlowLayout.Controls.Add(this.lblStockOutValue);
            this.summaryFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryFlowLayout.Location = new System.Drawing.Point(16, 8);
            this.summaryFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.summaryFlowLayout.Name = "summaryFlowLayout";
            this.summaryFlowLayout.Size = new System.Drawing.Size(868, 34);
            this.summaryFlowLayout.TabIndex = 0;
            // 
            // NEW: lblTotalSalesTitle
            // 
            this.lblTotalSalesTitle.AutoSize = true;
            this.lblTotalSalesTitle.Location = new System.Drawing.Point(0, 8);
            this.lblTotalSalesTitle.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblTotalSalesTitle.Name = "lblTotalSalesTitle";
            this.lblTotalSalesTitle.Size = new System.Drawing.Size(66, 15);
            this.lblTotalSalesTitle.TabIndex = 0;
            this.lblTotalSalesTitle.Text = "Total Sales:";
            // 
            // NEW: lblTotalSalesValue
            // 
            this.lblTotalSalesValue.AutoSize = true;
            this.lblTotalSalesValue.Location = new System.Drawing.Point(71, 8);
            this.lblTotalSalesValue.Margin = new System.Windows.Forms.Padding(0, 8, 20, 0);
            this.lblTotalSalesValue.Name = "lblTotalSalesValue";
            this.lblTotalSalesValue.Size = new System.Drawing.Size(61, 15);
            this.lblTotalSalesValue.TabIndex = 1;
            this.lblTotalSalesValue.Text = "LKR 0.00";
            // 
            // NEW: lblStockInTitle
            // 
            this.lblStockInTitle.AutoSize = true;
            this.lblStockInTitle.Location = new System.Drawing.Point(152, 8);
            this.lblStockInTitle.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblStockInTitle.Name = "lblStockInTitle";
            this.lblStockInTitle.Size = new System.Drawing.Size(84, 15);
            this.lblStockInTitle.TabIndex = 2;
            this.lblStockInTitle.Text = "Total Stock In:";
            this.lblStockInTitle.Visible = false; // Hidden by default
            // 
            // NEW: lblStockInValue
            // 
            this.lblStockInValue.AutoSize = true;
            this.lblStockInValue.Location = new System.Drawing.Point(241, 8);
            this.lblStockInValue.Margin = new System.Windows.Forms.Padding(0, 8, 20, 0);
            this.lblStockInValue.Name = "lblStockInValue";
            this.lblStockInValue.Size = new System.Drawing.Size(13, 15);
            this.lblStockInValue.TabIndex = 3;
            this.lblStockInValue.Text = "0";
            this.lblStockInValue.Visible = false; // Hidden by default
            // 
            // NEW: lblStockOutTitle
            // 
            this.lblStockOutTitle.AutoSize = true;
            this.lblStockOutTitle.Location = new System.Drawing.Point(274, 8);
            this.lblStockOutTitle.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblStockOutTitle.Name = "lblStockOutTitle";
            this.lblStockOutTitle.Size = new System.Drawing.Size(92, 15);
            this.lblStockOutTitle.TabIndex = 4;
            this.lblStockOutTitle.Text = "Total Stock Out:";
            this.lblStockOutTitle.Visible = false; // Hidden by default
            // 
            // NEW: lblStockOutValue
            // 
            this.lblStockOutValue.AutoSize = true;
            this.lblStockOutValue.Location = new System.Drawing.Point(371, 8);
            this.lblStockOutValue.Margin = new System.Windows.Forms.Padding(0, 8, 20, 0);
            this.lblStockOutValue.Name = "lblStockOutValue";
            this.lblStockOutValue.Size = new System.Drawing.Size(13, 15);
            this.lblStockOutValue.TabIndex = 5;
            this.lblStockOutValue.Text = "0";
            this.lblStockOutValue.Visible = false; // Hidden by default

            // 
            // reportPanel
            // 
            this.reportPanel.Controls.Add(this.gridReport);
            this.reportPanel.Controls.Add(this.lblEmpty);
            this.reportPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            // MODIFIED: Update Location and Margin
            this.reportPanel.Location = new System.Drawing.Point(16, 212); // New Y position
            this.reportPanel.Margin = new System.Windows.Forms.Padding(16);

            this.reportPanel.Name = "reportPanel";
            this.reportPanel.Padding = new System.Windows.Forms.Padding(16);
            this.reportPanel.Size = new System.Drawing.Size(868, 372); // Adjusted size
            this.reportPanel.TabIndex = 3; // Changed TabIndex
            // 
            // lblEmpty
            // 
            this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEmpty.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmpty.Location = new System.Drawing.Point(16, 16);
            this.lblEmpty.Name = "lblEmpty";
            this.lblEmpty.Size = new System.Drawing.Size(836, 40);
            this.lblEmpty.TabIndex = 1;
            this.lblEmpty.Text = "Select a report type, month, and (optionally) a date, then click Generate.";
            this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridReport
            // 
            this.gridReport.AllowUserToAddRows = false;
            this.gridReport.AllowUserToDeleteRows = false;
            this.gridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridReport.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.gridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReport.EnableHeadersVisualStyles = false;
            this.gridReport.Location = new System.Drawing.Point(16, 56);
            this.gridReport.Name = "gridReport";
            this.gridReport.ReadOnly = true;
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.Size = new System.Drawing.Size(836, 300); // Adjusted size
            this.gridReport.TabIndex = 0;
            // 
            // ReportsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.root);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportsControl";
            this.Size = new System.Drawing.Size(900, 600);

            // Resume layout
            this.root.ResumeLayout(false);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsRow.ResumeLayout(false);
            this.controlsRow.PerformLayout();

            // NEW: Resume layout for new panel
            this.summaryPanel.ResumeLayout(false);
            this.summaryFlowLayout.ResumeLayout(false);
            this.summaryFlowLayout.PerformLayout();

            this.reportPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion
    }
}