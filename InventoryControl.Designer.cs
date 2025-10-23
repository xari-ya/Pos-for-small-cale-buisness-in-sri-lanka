// -----------------------------------------------------------------------------
// InventoryControl.Designer.cs
// -----------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace billing_system
{
    partial class InventoryControl
    {
        private IContainer components = null;

        // Designer-managed fields
        private Panel headerPanel;
        private Label lblTitle;
        private Panel filterPanel;
        private TextBox txtSearch;
        private ComboBox cbCategory;
        private NumericUpDown numMinStock;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private DataGridView dgvProducts;
        private Panel summaryPanel;
        private Label lblSummaryTitle;
        private Label lblTotalProducts;
        private Label lblLowStock;
        private Label lblOutOfStock;
        private Label lblInventoryValue;

        /// <summary>Dispose resources</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>Designer code — Initialize UI</summary>
        /// <summary>Designer code — Initialize UI</summary>
        private void InitializeComponent()
        {
            headerPanel = new Panel();
            lblTitle = new Label();
            filterPanel = new Panel();
            flowFilters = new FlowLayoutPanel();
            txtSearch = new TextBox();
            cbCategory = new ComboBox();
            numMinStock = new NumericUpDown();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            summaryPanel = new Panel();
            flowSummary = new FlowLayoutPanel();
            lblSummaryTitle = new Label();
            lblTotalProducts = new Label();
            lblLowStock = new Label();
            lblOutOfStock = new Label();
            lblInventoryValue = new Label();
            dgvProducts = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            headerPanel.SuspendLayout();
            filterPanel.SuspendLayout();
            flowFilters.SuspendLayout();
            ((ISupportInitialize)numMinStock).BeginInit();
            summaryPanel.SuspendLayout();
            flowSummary.SuspendLayout();
            ((ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(44, 62, 80);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(16, 10, 16, 10);
            headerPanel.Size = new Size(1000, 60);
            headerPanel.TabIndex = 3;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(188, 51);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Inventory";
            // 
            // filterPanel
            // 
            filterPanel.BackColor = Color.White;
            filterPanel.Controls.Add(flowFilters);
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Location = new Point(0, 60);
            filterPanel.Name = "filterPanel";
            filterPanel.Padding = new Padding(20, 10, 20, 10);
            filterPanel.Size = new Size(1000, 70);
            filterPanel.TabIndex = 2;
            // 
            // flowFilters
            // 
            flowFilters.AutoScroll = true;
            flowFilters.Controls.Add(txtSearch);
            flowFilters.Controls.Add(cbCategory);
            flowFilters.Controls.Add(numMinStock);
            flowFilters.Controls.Add(btnAdd);
            flowFilters.Controls.Add(btnEdit);
            flowFilters.Controls.Add(btnDelete);
            flowFilters.Dock = DockStyle.Fill;
            flowFilters.Location = new Point(20, 10);
            flowFilters.Margin = new Padding(0);
            flowFilters.Name = "flowFilters";
            flowFilters.Size = new Size(960, 50);
            flowFilters.TabIndex = 0;
            flowFilters.WrapContents = false;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(0, 5);
            txtSearch.Margin = new Padding(0, 5, 10, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 35);
            txtSearch.TabIndex = 0;
            // 
            // cbCategory
            // 
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.Items.AddRange(new object[] { "All", "Accessories", "Peripherals", "Office", "Other" });
            cbCategory.Location = new Point(210, 5);
            cbCategory.Margin = new Padding(0, 5, 10, 5);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(150, 38);
            cbCategory.TabIndex = 1;
            // 
            // numMinStock
            // 
            numMinStock.Location = new Point(370, 5);
            numMinStock.Margin = new Padding(0, 5, 10, 5);
            numMinStock.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numMinStock.Name = "numMinStock";
            numMinStock.Size = new Size(80, 35);
            numMinStock.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(460, 5);
            btnAdd.Margin = new Padding(0, 5, 10, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(90, 35);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(52, 73, 94);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(560, 5);
            btnEdit.Margin = new Padding(0, 5, 10, 5);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 35);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(660, 5);
            btnDelete.Margin = new Padding(0, 5, 10, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 35);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // summaryPanel
            // 
            summaryPanel.BackColor = Color.White;
            summaryPanel.Controls.Add(flowSummary);
            summaryPanel.Dock = DockStyle.Top;
            summaryPanel.Location = new Point(0, 130);
            summaryPanel.Name = "summaryPanel";
            summaryPanel.Padding = new Padding(20, 0, 20, 0);
            summaryPanel.Size = new Size(1000, 50);
            summaryPanel.TabIndex = 1;
            // 
            // flowSummary
            // 
            flowSummary.AutoScroll = true;
            flowSummary.Controls.Add(lblSummaryTitle);
            flowSummary.Controls.Add(lblTotalProducts);
            flowSummary.Controls.Add(lblLowStock);
            flowSummary.Controls.Add(lblOutOfStock);
            flowSummary.Controls.Add(lblInventoryValue);
            flowSummary.Dock = DockStyle.Fill;
            flowSummary.Location = new Point(20, 0);
            flowSummary.Margin = new Padding(0);
            flowSummary.Name = "flowSummary";
            flowSummary.Size = new Size(960, 50);
            flowSummary.TabIndex = 0;
            flowSummary.WrapContents = false;
            // 
            // lblSummaryTitle
            // 
            lblSummaryTitle.AutoSize = true;
            lblSummaryTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSummaryTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblSummaryTitle.Location = new Point(0, 12);
            lblSummaryTitle.Margin = new Padding(0, 12, 25, 0);
            lblSummaryTitle.Name = "lblSummaryTitle";
            lblSummaryTitle.Size = new Size(215, 38);
            lblSummaryTitle.TabIndex = 0;
            lblSummaryTitle.Text = "Stock Summary";
            // 
            // lblTotalProducts
            // 
            lblTotalProducts.AutoSize = true;
            lblTotalProducts.Font = new Font("Segoe UI", 10F);
            lblTotalProducts.Location = new Point(240, 15);
            lblTotalProducts.Margin = new Padding(0, 15, 25, 0);
            lblTotalProducts.Name = "lblTotalProducts";
            lblTotalProducts.Size = new Size(228, 32);
            lblTotalProducts.TabIndex = 1;
            lblTotalProducts.Text = "Total Products: 1245";
            // 
            // lblLowStock
            // 
            lblLowStock.AutoSize = true;
            lblLowStock.Font = new Font("Segoe UI", 10F);
            lblLowStock.Location = new Point(493, 15);
            lblLowStock.Margin = new Padding(0, 15, 25, 0);
            lblLowStock.Name = "lblLowStock";
            lblLowStock.Size = new Size(158, 32);
            lblLowStock.TabIndex = 2;
            lblLowStock.Text = "Low Stock: 32";
            // 
            // lblOutOfStock
            // 
            lblOutOfStock.AutoSize = true;
            lblOutOfStock.Font = new Font("Segoe UI", 10F);
            lblOutOfStock.Location = new Point(676, 15);
            lblOutOfStock.Margin = new Padding(0, 15, 25, 0);
            lblOutOfStock.Name = "lblOutOfStock";
            lblOutOfStock.Size = new Size(0, 32);
            lblOutOfStock.TabIndex = 3;
            // 
            // lblInventoryValue
            // 
            lblInventoryValue.AutoSize = true;
            lblInventoryValue.Font = new Font("Segoe UI", 10F);
            lblInventoryValue.Location = new Point(701, 15);
            lblInventoryValue.Margin = new Padding(0, 15, 25, 0);
            lblInventoryValue.Name = "lblInventoryValue";
            lblInventoryValue.Size = new Size(275, 32);
            lblInventoryValue.TabIndex = 4;
            lblInventoryValue.Text = "Inventory Value: $12,430";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.ColumnHeadersHeight = 40;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6 });
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.Location = new Point(0, 180);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 72;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(1000, 520);
            dgvProducts.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "SKU";
            dataGridViewTextBoxColumn1.MinimumWidth = 9;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Name";
            dataGridViewTextBoxColumn2.MinimumWidth = 9;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Category";
            dataGridViewTextBoxColumn3.MinimumWidth = 9;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Price";
            dataGridViewTextBoxColumn4.MinimumWidth = 9;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Stock";
            dataGridViewTextBoxColumn5.MinimumWidth = 9;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Reorder";
            dataGridViewTextBoxColumn6.MinimumWidth = 9;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // InventoryControl
            // 
            BackColor = Color.FromArgb(244, 246, 247);
            Controls.Add(dgvProducts);
            Controls.Add(summaryPanel);
            Controls.Add(filterPanel);
            Controls.Add(headerPanel);
            Name = "InventoryControl";
            Size = new Size(1000, 700);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            filterPanel.ResumeLayout(false);
            flowFilters.ResumeLayout(false);
            flowFilters.PerformLayout();
            ((ISupportInitialize)numMinStock).EndInit();
            summaryPanel.ResumeLayout(false);
            flowSummary.ResumeLayout(false);
            flowSummary.PerformLayout();
            ((ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private FlowLayoutPanel flowFilters;
        private FlowLayoutPanel flowSummary;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}
