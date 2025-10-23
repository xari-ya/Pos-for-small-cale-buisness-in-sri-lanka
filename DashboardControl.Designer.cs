namespace billing_system
{
    partial class DashboardControl
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            headerPanel = new Panel();
            lblWelcomeUser = new Label();
            lblTitle = new Label();
            kpiTableLayout = new TableLayoutPanel();
            panelSales = new Panel();
            panelInventory = new Panel();
            panelAttention = new Panel();
            iconSales = new PictureBox();
            lblSalesTitle = new Label();
            lblSalesValue = new Label();
            iconInventory = new PictureBox();
            lblInventoryTitle = new Label();
            lblInventoryValue = new Label();
            iconAttention = new PictureBox();
            lblAttentionTitle = new Label();
            lblAttentionValue = new Label();
            lblRecentTransactions = new Label();
            dgvRecentTransactions = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            headerPanel.SuspendLayout();
            kpiTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconInventory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconAttention).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRecentTransactions).BeginInit();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(lblWelcomeUser);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(20, 20);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(560, 60);
            headerPanel.TabIndex = 3;
            // 
            // lblWelcomeUser
            // 
            lblWelcomeUser.Dock = DockStyle.Right;
            lblWelcomeUser.Font = new Font("Segoe UI", 12F);
            lblWelcomeUser.Location = new Point(260, 0);
            lblWelcomeUser.Name = "lblWelcomeUser";
            lblWelcomeUser.Size = new Size(300, 60);
            lblWelcomeUser.TabIndex = 0;
            lblWelcomeUser.Text = "Welcome, [Username]";
            lblWelcomeUser.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 60);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Dashboard Overview";
            // 
            // kpiTableLayout
            // 
            kpiTableLayout.ColumnCount = 3;
            kpiTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            kpiTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            kpiTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            kpiTableLayout.Controls.Add(panelSales, 0, 0);
            kpiTableLayout.Controls.Add(panelInventory, 1, 0);
            kpiTableLayout.Controls.Add(panelAttention, 2, 0);
            kpiTableLayout.Dock = DockStyle.Top;
            kpiTableLayout.Location = new Point(20, 80);
            kpiTableLayout.Name = "kpiTableLayout";
            kpiTableLayout.RowCount = 1;
            kpiTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            kpiTableLayout.Size = new Size(560, 200);
            kpiTableLayout.TabIndex = 0;
            // 
            // panelSales
            // 
            panelSales.Location = new Point(3, 3);
            panelSales.Name = "panelSales";
            panelSales.Size = new Size(180, 100);
            panelSales.TabIndex = 0;
            // 
            // panelInventory
            // 
            panelInventory.Location = new Point(189, 3);
            panelInventory.Name = "panelInventory";
            panelInventory.Size = new Size(180, 100);
            panelInventory.TabIndex = 1;
            // 
            // panelAttention
            // 
            panelAttention.Location = new Point(375, 3);
            panelAttention.Name = "panelAttention";
            panelAttention.Size = new Size(182, 100);
            panelAttention.TabIndex = 2;
            // 
            // iconSales
            // 
            iconSales.Location = new Point(0, 0);
            iconSales.Name = "iconSales";
            iconSales.Size = new Size(100, 50);
            iconSales.TabIndex = 0;
            iconSales.TabStop = false;
            // 
            // lblSalesTitle
            // 
            lblSalesTitle.Location = new Point(0, 0);
            lblSalesTitle.Name = "lblSalesTitle";
            lblSalesTitle.Size = new Size(100, 23);
            lblSalesTitle.TabIndex = 0;
            // 
            // lblSalesValue
            // 
            lblSalesValue.Location = new Point(0, 0);
            lblSalesValue.Name = "lblSalesValue";
            lblSalesValue.Size = new Size(100, 23);
            lblSalesValue.TabIndex = 0;
            // 
            // iconInventory
            // 
            iconInventory.Location = new Point(0, 0);
            iconInventory.Name = "iconInventory";
            iconInventory.Size = new Size(100, 50);
            iconInventory.TabIndex = 0;
            iconInventory.TabStop = false;
            // 
            // lblInventoryTitle
            // 
            lblInventoryTitle.Location = new Point(0, 0);
            lblInventoryTitle.Name = "lblInventoryTitle";
            lblInventoryTitle.Size = new Size(100, 23);
            lblInventoryTitle.TabIndex = 0;
            // 
            // lblInventoryValue
            // 
            lblInventoryValue.Location = new Point(0, 0);
            lblInventoryValue.Name = "lblInventoryValue";
            lblInventoryValue.Size = new Size(100, 23);
            lblInventoryValue.TabIndex = 0;
            // 
            // iconAttention
            // 
            iconAttention.Location = new Point(0, 0);
            iconAttention.Name = "iconAttention";
            iconAttention.Size = new Size(100, 50);
            iconAttention.TabIndex = 0;
            iconAttention.TabStop = false;
            // 
            // lblAttentionTitle
            // 
            lblAttentionTitle.Location = new Point(0, 0);
            lblAttentionTitle.Name = "lblAttentionTitle";
            lblAttentionTitle.Size = new Size(100, 23);
            lblAttentionTitle.TabIndex = 0;
            // 
            // lblAttentionValue
            // 
            lblAttentionValue.Location = new Point(0, 0);
            lblAttentionValue.Name = "lblAttentionValue";
            lblAttentionValue.Size = new Size(100, 23);
            lblAttentionValue.TabIndex = 0;
            // 
            // lblRecentTransactions
            // 
            lblRecentTransactions.Dock = DockStyle.Top;
            lblRecentTransactions.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblRecentTransactions.Location = new Point(20, 280);
            lblRecentTransactions.Name = "lblRecentTransactions";
            lblRecentTransactions.Padding = new Padding(0, 20, 0, 10);
            lblRecentTransactions.Size = new Size(560, 23);
            lblRecentTransactions.TabIndex = 2;
            lblRecentTransactions.Text = "Recent Transactions";
            // 
            // dgvRecentTransactions
            // 
            dgvRecentTransactions.AllowUserToAddRows = false;
            dgvRecentTransactions.AllowUserToDeleteRows = false;
            dgvRecentTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentTransactions.BackgroundColor = Color.White;
            dgvRecentTransactions.BorderStyle = BorderStyle.None;
            dgvRecentTransactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentTransactions.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5 });
            dgvRecentTransactions.Dock = DockStyle.Fill;
            dgvRecentTransactions.Location = new Point(20, 303);
            dgvRecentTransactions.Name = "dgvRecentTransactions";
            dgvRecentTransactions.ReadOnly = true;
            dgvRecentTransactions.RowHeadersWidth = 72;
            dgvRecentTransactions.Size = new Size(560, 177);
            dgvRecentTransactions.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Invoice ID";
            dataGridViewTextBoxColumn1.MinimumWidth = 9;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Date & Time";
            dataGridViewTextBoxColumn2.MinimumWidth = 9;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Items";
            dataGridViewTextBoxColumn3.MinimumWidth = 9;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Total";
            dataGridViewTextBoxColumn4.MinimumWidth = 9;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Method";
            dataGridViewTextBoxColumn5.MinimumWidth = 9;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // DashboardControl
            // 
            BackColor = Color.FromArgb(245, 246, 250);
            Controls.Add(dgvRecentTransactions);
            Controls.Add(lblRecentTransactions);
            Controls.Add(kpiTableLayout);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9.75F);
            MinimumSize = new Size(600, 500);
            Name = "DashboardControl";
            Padding = new Padding(20);
            Size = new Size(600, 500);
            headerPanel.ResumeLayout(false);
            kpiTableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconInventory).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconAttention).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRecentTransactions).EndInit();
            ResumeLayout(false);
        }

        private void CreateKpiPanel(Panel panel, PictureBox icon, Label title, Label value, string titleText, string valueText, Color valueColor)
        {
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(15);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            // Inner layout for neat vertical stacking
            var innerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10, 15, 10, 15)
            };
            innerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));  // icon
            innerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // title
            innerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));  // value

            // Icon setup
            icon.Dock = DockStyle.Fill;
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.Margin = new Padding(25, 5, 25, 5);
            icon.BackColor = Color.Transparent;

            // You can later assign icons in the logic file, e.g.:
            // iconSales.Image = Properties.Resources.sales_icon;

            // Title label
            title.Text = titleText;
            title.ForeColor = Color.Gray;
            title.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            title.Dock = DockStyle.Fill;
            title.TextAlign = ContentAlignment.MiddleCenter;

            // Value label
            value.Text = valueText;
            value.ForeColor = valueColor;
            value.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            value.Dock = DockStyle.Fill;
            value.TextAlign = ContentAlignment.MiddleCenter;

            // Add controls
            innerLayout.Controls.Add(icon, 0, 0);
            innerLayout.Controls.Add(title, 0, 1);
            innerLayout.Controls.Add(value, 0, 2);

            panel.Controls.Add(innerLayout);
        }
        #endregion

        private Panel headerPanel;
        private Label lblTitle;
        private Label lblWelcomeUser;
        private TableLayoutPanel kpiTableLayout;
        private Panel panelSales;
        private PictureBox iconSales;
        private Label lblSalesTitle;
        private Label lblSalesValue;
        private Panel panelInventory;
        private PictureBox iconInventory;
        private Label lblInventoryTitle;
        private Label lblInventoryValue;
        private Panel panelAttention;
        private PictureBox iconAttention;
        private Label lblAttentionTitle;
        private Label lblAttentionValue;
        private Label lblRecentTransactions;
        private DataGridView dgvRecentTransactions;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
