namespace billing_system
{
    partial class CashierPOSForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            headerPanel = new Panel();
            welcomeFlowLayoutPanel = new FlowLayoutPanel();
            logoutButton = new Button();
            userProfileButton = new Button();
            welcomeLabel = new Label();
            bimsLabel = new Label();
            subHeaderPanel = new Panel();
            subHeaderButtonPanel = new FlowLayoutPanel();
            cancelTransactionButton = new Button();
            removeItemButton = new Button();
            invoiceLabel = new Label();
            mainContentPanel = new Panel();
            mainTableLayoutPanel = new TableLayoutPanel();
            leftPanel = new Panel();
            addItemContainerPanel = new Panel();
            addToBillButton = new Button();
            quantityNumericUpDown = new NumericUpDown();
            quantityLabel = new Label();
            searchResultsListBox = new ListBox();
            searchTextBox = new TextBox();
            scanLabel = new Label();
            rightPanel = new Panel();
            billAndTotalsSplitter = new TableLayoutPanel();
            billItemsSectionPanel = new Panel();
            billItemsFlowPanel = new FlowLayoutPanel();
            currentBillLabel = new Label();
            totalsSectionPanel = new Panel();
            finalizeAndPayButton = new Button();
            totalDueValueLabel = new Label();
            totalDueTextLabel = new Label();
            separatorPanel = new Panel();
            discountFlowLayoutPanel = new FlowLayoutPanel();
            discountTypeComboBox = new ComboBox();
            discountNumericUpDown = new NumericUpDown();
            applyDiscountButton = new Button();
            totalsTableLayoutPanel = new TableLayoutPanel();
            subtotalTextLabel = new Label();
            subtotalValueLabel = new Label();
            discountTextLabel = new Label();
            discountValueLabel = new Label();
            headerPanel.SuspendLayout();
            welcomeFlowLayoutPanel.SuspendLayout();
            subHeaderPanel.SuspendLayout();
            subHeaderButtonPanel.SuspendLayout();
            mainContentPanel.SuspendLayout();
            mainTableLayoutPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            addItemContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)quantityNumericUpDown).BeginInit();
            rightPanel.SuspendLayout();
            billAndTotalsSplitter.SuspendLayout();
            billItemsSectionPanel.SuspendLayout();
            totalsSectionPanel.SuspendLayout();
            discountFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)discountNumericUpDown).BeginInit();
            totalsTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(45, 52, 71);
            headerPanel.Controls.Add(welcomeFlowLayoutPanel);
            headerPanel.Controls.Add(bimsLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(4);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(30, 0, 30, 0);
            headerPanel.Size = new Size(2880, 104);
            headerPanel.TabIndex = 2;
            // 
            // welcomeFlowLayoutPanel
            // 
            welcomeFlowLayoutPanel.Controls.Add(logoutButton);
            welcomeFlowLayoutPanel.Controls.Add(userProfileButton);
            welcomeFlowLayoutPanel.Controls.Add(welcomeLabel);
            welcomeFlowLayoutPanel.Dock = DockStyle.Right;
            welcomeFlowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
            welcomeFlowLayoutPanel.Location = new Point(2175, 0);
            welcomeFlowLayoutPanel.Margin = new Padding(4);
            welcomeFlowLayoutPanel.Name = "welcomeFlowLayoutPanel";
            welcomeFlowLayoutPanel.Size = new Size(675, 104);
            welcomeFlowLayoutPanel.TabIndex = 0;
            // 
            // logoutButton
            // 
            logoutButton.Anchor = AnchorStyles.Right;
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.FlatStyle = FlatStyle.Flat;
            logoutButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            logoutButton.ForeColor = Color.White;
            logoutButton.Location = new Point(596, 4);
            logoutButton.Margin = new Padding(4);
            logoutButton.Name = "logoutButton";
            logoutButton.Size = new Size(75, 69);
            logoutButton.TabIndex = 0;
            logoutButton.Text = "->";
            // 
            // userProfileButton
            // 
            userProfileButton.Anchor = AnchorStyles.Right;
            userProfileButton.FlatAppearance.BorderSize = 0;
            userProfileButton.FlatStyle = FlatStyle.Flat;
            userProfileButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            userProfileButton.ForeColor = Color.White;
            userProfileButton.Location = new Point(513, 4);
            userProfileButton.Margin = new Padding(4);
            userProfileButton.Name = "userProfileButton";
            userProfileButton.Size = new Size(75, 69);
            userProfileButton.TabIndex = 1;
            userProfileButton.Text = "U";
            // 
            // welcomeLabel
            // 
            welcomeLabel.Anchor = AnchorStyles.Right;
            welcomeLabel.AutoSize = true;
            welcomeLabel.Font = new Font("Segoe UI", 12F);
            welcomeLabel.ForeColor = Color.White;
            welcomeLabel.Location = new Point(242, 19);
            welcomeLabel.Margin = new Padding(4, 0, 30, 0);
            welcomeLabel.Name = "welcomeLabel";
            welcomeLabel.Size = new Size(237, 38);
            welcomeLabel.TabIndex = 2;
            welcomeLabel.Text = "Welcome, Cashier";
            // 
            // bimsLabel
            // 
            bimsLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            bimsLabel.ForeColor = Color.White;
            bimsLabel.Location = new Point(30, 0);
            bimsLabel.Margin = new Padding(4, 0, 4, 0);
            bimsLabel.Name = "bimsLabel";
            bimsLabel.Size = new Size(225, 104);
            bimsLabel.TabIndex = 1;
            bimsLabel.Text = "BIMS";
            bimsLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // subHeaderPanel
            // 
            subHeaderPanel.Controls.Add(subHeaderButtonPanel);
            subHeaderPanel.Controls.Add(invoiceLabel);
            subHeaderPanel.Dock = DockStyle.Top;
            subHeaderPanel.Location = new Point(0, 104);
            subHeaderPanel.Margin = new Padding(4);
            subHeaderPanel.Name = "subHeaderPanel";
            subHeaderPanel.Padding = new Padding(30, 0, 30, 0);
            subHeaderPanel.Size = new Size(2880, 96);
            subHeaderPanel.TabIndex = 1;
            // 
            // subHeaderButtonPanel
            // 
            subHeaderButtonPanel.Controls.Add(cancelTransactionButton);
            subHeaderButtonPanel.Controls.Add(removeItemButton);
            subHeaderButtonPanel.Dock = DockStyle.Right;
            subHeaderButtonPanel.FlowDirection = FlowDirection.RightToLeft;
            subHeaderButtonPanel.Location = new Point(2325, 0);
            subHeaderButtonPanel.Margin = new Padding(4);
            subHeaderButtonPanel.Name = "subHeaderButtonPanel";
            subHeaderButtonPanel.Padding = new Padding(0, 14, 0, 14);
            subHeaderButtonPanel.Size = new Size(525, 96);
            subHeaderButtonPanel.TabIndex = 0;
            // 
            // cancelTransactionButton
            // 
            cancelTransactionButton.Font = new Font("Segoe UI", 10F);
            cancelTransactionButton.Location = new Point(296, 14);
            cancelTransactionButton.Margin = new Padding(4, 0, 4, 0);
            cancelTransactionButton.Name = "cancelTransactionButton";
            cancelTransactionButton.Size = new Size(225, 69);
            cancelTransactionButton.TabIndex = 0;
            cancelTransactionButton.Text = "Cancel Transaction";
            // 
            // removeItemButton
            // 
            removeItemButton.Font = new Font("Segoe UI", 10F);
            removeItemButton.Location = new Point(63, 14);
            removeItemButton.Margin = new Padding(4, 0, 4, 0);
            removeItemButton.Name = "removeItemButton";
            removeItemButton.Size = new Size(225, 69);
            removeItemButton.TabIndex = 1;
            removeItemButton.Text = "Remove Item";
            // 
            // invoiceLabel
            // 
            invoiceLabel.Font = new Font("Segoe UI", 12F);
            invoiceLabel.Location = new Point(30, 0);
            invoiceLabel.Margin = new Padding(4, 0, 4, 0);
            invoiceLabel.Name = "invoiceLabel";
            invoiceLabel.Size = new Size(300, 96);
            invoiceLabel.TabIndex = 1;
            invoiceLabel.Text = "Invoice #: 1001";
            invoiceLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mainContentPanel
            // 
            mainContentPanel.BackColor = SystemColors.ControlLightLight;
            mainContentPanel.Controls.Add(mainTableLayoutPanel);
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.Location = new Point(0, 200);
            mainContentPanel.Margin = new Padding(4);
            mainContentPanel.Name = "mainContentPanel";
            mainContentPanel.Padding = new Padding(30, 34, 30, 34);
            mainContentPanel.Size = new Size(2880, 1388);
            mainContentPanel.TabIndex = 0;
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.ColumnCount = 2;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            mainTableLayoutPanel.Controls.Add(leftPanel, 0, 0);
            mainTableLayoutPanel.Controls.Add(rightPanel, 1, 0);
            mainTableLayoutPanel.Dock = DockStyle.Fill;
            mainTableLayoutPanel.Location = new Point(30, 34);
            mainTableLayoutPanel.Margin = new Padding(4);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 1;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.Size = new Size(2820, 1320);
            mainTableLayoutPanel.TabIndex = 0;
            // 
            // leftPanel
            // 
            leftPanel.BackColor = SystemColors.Control;
            leftPanel.BorderStyle = BorderStyle.FixedSingle;
            leftPanel.Controls.Add(addItemContainerPanel);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.Location = new Point(4, 4);
            leftPanel.Margin = new Padding(4, 4, 15, 4);
            leftPanel.Name = "leftPanel";
            leftPanel.Padding = new Padding(30, 34, 30, 34);
            leftPanel.Size = new Size(968, 1312);
            leftPanel.TabIndex = 0;
            // 
            // addItemContainerPanel
            // 
            addItemContainerPanel.Controls.Add(addToBillButton);
            addItemContainerPanel.Controls.Add(quantityNumericUpDown);
            addItemContainerPanel.Controls.Add(quantityLabel);
            addItemContainerPanel.Controls.Add(searchResultsListBox);
            addItemContainerPanel.Controls.Add(searchTextBox);
            addItemContainerPanel.Controls.Add(scanLabel);
            addItemContainerPanel.Dock = DockStyle.Top;
            addItemContainerPanel.Location = new Point(30, 34);
            addItemContainerPanel.Margin = new Padding(4);
            addItemContainerPanel.Name = "addItemContainerPanel";
            addItemContainerPanel.Size = new Size(906, 778);
            addItemContainerPanel.TabIndex = 0;
            // 
            // addToBillButton
            // 
            addToBillButton.BackColor = Color.MediumSeaGreen;
            addToBillButton.Dock = DockStyle.Top;
            addToBillButton.FlatAppearance.BorderSize = 0;
            addToBillButton.FlatStyle = FlatStyle.Flat;
            addToBillButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            addToBillButton.ForeColor = Color.White;
            addToBillButton.Location = new Point(0, 609);
            addToBillButton.Margin = new Padding(6, 50, 6, 7);
            addToBillButton.Name = "addToBillButton";
            addToBillButton.Size = new Size(906, 127);
            addToBillButton.TabIndex = 5;
            addToBillButton.Text = "Add to Bill";
            addToBillButton.UseVisualStyleBackColor = false;
            // 
            // quantityNumericUpDown
            // 
            quantityNumericUpDown.Dock = DockStyle.Top;
            quantityNumericUpDown.Font = new Font("Segoe UI", 14F);
            quantityNumericUpDown.Location = new Point(0, 558);
            quantityNumericUpDown.Margin = new Padding(4);
            quantityNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            quantityNumericUpDown.Name = "quantityNumericUpDown";
            quantityNumericUpDown.Size = new Size(906, 51);
            quantityNumericUpDown.TabIndex = 1;
            quantityNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Dock = DockStyle.Top;
            quantityLabel.Font = new Font("Segoe UI", 12F);
            quantityLabel.Location = new Point(0, 484);
            quantityLabel.Margin = new Padding(4, 0, 4, 0);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Padding = new Padding(0, 18, 0, 18);
            quantityLabel.Size = new Size(123, 74);
            quantityLabel.TabIndex = 2;
            quantityLabel.Text = "Quantity";
            // 
            // searchResultsListBox
            // 
            searchResultsListBox.Dock = DockStyle.Top;
            searchResultsListBox.Font = new Font("Segoe UI", 14F);
            searchResultsListBox.ItemHeight = 45;
            searchResultsListBox.Location = new Point(0, 120);
            searchResultsListBox.Margin = new Padding(4);
            searchResultsListBox.Name = "searchResultsListBox";
            searchResultsListBox.Size = new Size(906, 364);
            searchResultsListBox.TabIndex = 3;
            searchResultsListBox.Visible = false;
            // 
            // searchTextBox
            // 
            searchTextBox.Dock = DockStyle.Top;
            searchTextBox.Font = new Font("Segoe UI", 14F);
            searchTextBox.Location = new Point(0, 69);
            searchTextBox.Margin = new Padding(4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(906, 51);
            searchTextBox.TabIndex = 4;
            // 
            // scanLabel
            // 
            scanLabel.AutoSize = true;
            scanLabel.Dock = DockStyle.Top;
            scanLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            scanLabel.Location = new Point(0, 0);
            scanLabel.Margin = new Padding(4, 0, 4, 0);
            scanLabel.Name = "scanLabel";
            scanLabel.Padding = new Padding(0, 0, 0, 18);
            scanLabel.Size = new Size(458, 69);
            scanLabel.TabIndex = 5;
            scanLabel.Text = "Scan or search product...";
            // 
            // rightPanel
            // 
            rightPanel.BackColor = SystemColors.Control;
            rightPanel.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(billAndTotalsSplitter);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(1002, 4);
            rightPanel.Margin = new Padding(15, 4, 4, 4);
            rightPanel.Name = "rightPanel";
            rightPanel.Padding = new Padding(2);
            rightPanel.Size = new Size(1814, 1312);
            rightPanel.TabIndex = 1;
            // 
            // billAndTotalsSplitter
            // 
            billAndTotalsSplitter.ColumnCount = 2;
            billAndTotalsSplitter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            billAndTotalsSplitter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            billAndTotalsSplitter.Controls.Add(billItemsSectionPanel, 0, 0);
            billAndTotalsSplitter.Controls.Add(totalsSectionPanel, 1, 0);
            billAndTotalsSplitter.Dock = DockStyle.Fill;
            billAndTotalsSplitter.Location = new Point(2, 2);
            billAndTotalsSplitter.Margin = new Padding(4);
            billAndTotalsSplitter.Name = "billAndTotalsSplitter";
            billAndTotalsSplitter.RowCount = 1;
            billAndTotalsSplitter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            billAndTotalsSplitter.Size = new Size(1808, 1306);
            billAndTotalsSplitter.TabIndex = 0;
            // 
            // billItemsSectionPanel
            // 
            billItemsSectionPanel.Controls.Add(billItemsFlowPanel);
            billItemsSectionPanel.Controls.Add(currentBillLabel);
            billItemsSectionPanel.Dock = DockStyle.Fill;
            billItemsSectionPanel.Location = new Point(4, 4);
            billItemsSectionPanel.Margin = new Padding(4);
            billItemsSectionPanel.Name = "billItemsSectionPanel";
            billItemsSectionPanel.Padding = new Padding(30, 34, 30, 34);
            billItemsSectionPanel.Size = new Size(1167, 1298);
            billItemsSectionPanel.TabIndex = 0;
            // 
            // billItemsFlowPanel
            // 
            billItemsFlowPanel.AutoScroll = true;
            billItemsFlowPanel.Dock = DockStyle.Fill;
            billItemsFlowPanel.FlowDirection = FlowDirection.TopDown;
            billItemsFlowPanel.Location = new Point(30, 103);
            billItemsFlowPanel.Margin = new Padding(4);
            billItemsFlowPanel.Name = "billItemsFlowPanel";
            billItemsFlowPanel.Size = new Size(1107, 1161);
            billItemsFlowPanel.TabIndex = 0;
            billItemsFlowPanel.WrapContents = false;
            // 
            // currentBillLabel
            // 
            currentBillLabel.AutoSize = true;
            currentBillLabel.Dock = DockStyle.Top;
            currentBillLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            currentBillLabel.Location = new Point(30, 34);
            currentBillLabel.Margin = new Padding(4, 0, 4, 0);
            currentBillLabel.Name = "currentBillLabel";
            currentBillLabel.Padding = new Padding(0, 0, 0, 18);
            currentBillLabel.Size = new Size(225, 69);
            currentBillLabel.TabIndex = 1;
            currentBillLabel.Text = "Current Bill";
            // 
            // totalsSectionPanel
            // 
            totalsSectionPanel.BackColor = SystemColors.ControlLightLight;
            totalsSectionPanel.Controls.Add(finalizeAndPayButton);
            totalsSectionPanel.Controls.Add(totalDueValueLabel);
            totalsSectionPanel.Controls.Add(totalDueTextLabel);
            totalsSectionPanel.Controls.Add(separatorPanel);
            totalsSectionPanel.Controls.Add(discountFlowLayoutPanel);
            totalsSectionPanel.Controls.Add(totalsTableLayoutPanel);
            totalsSectionPanel.Dock = DockStyle.Fill;
            totalsSectionPanel.Location = new Point(1175, 0);
            totalsSectionPanel.Margin = new Padding(0);
            totalsSectionPanel.Name = "totalsSectionPanel";
            totalsSectionPanel.Padding = new Padding(30, 34, 30, 34);
            totalsSectionPanel.Size = new Size(633, 1306);
            totalsSectionPanel.TabIndex = 1;
            // 
            // finalizeAndPayButton
            // 
            finalizeAndPayButton.BackColor = Color.MediumSeaGreen;
            finalizeAndPayButton.Dock = DockStyle.Bottom;
            finalizeAndPayButton.FlatAppearance.BorderSize = 0;
            finalizeAndPayButton.FlatStyle = FlatStyle.Flat;
            finalizeAndPayButton.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            finalizeAndPayButton.ForeColor = Color.White;
            finalizeAndPayButton.Location = new Point(30, 1145);
            finalizeAndPayButton.Margin = new Padding(6, 7, 6, 7);
            finalizeAndPayButton.Name = "finalizeAndPayButton";
            finalizeAndPayButton.Size = new Size(573, 127);
            finalizeAndPayButton.TabIndex = 4;
            finalizeAndPayButton.Text = "FINALIZE && PAY";
            finalizeAndPayButton.UseVisualStyleBackColor = false;
            finalizeAndPayButton.Click += finalizeAndPayButton_Click_1;
            // 
            // totalDueValueLabel
            // 
            totalDueValueLabel.AutoSize = true;
            totalDueValueLabel.Dock = DockStyle.Top;
            totalDueValueLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            totalDueValueLabel.Location = new Point(30, 341);
            totalDueValueLabel.Margin = new Padding(4, 0, 4, 0);
            totalDueValueLabel.Name = "totalDueValueLabel";
            totalDueValueLabel.Padding = new Padding(0, 18, 0, 0);
            totalDueValueLabel.Size = new Size(262, 130);
            totalDueValueLabel.TabIndex = 1;
            totalDueValueLabel.Text = "$4.25";
            // 
            // totalDueTextLabel
            // 
            totalDueTextLabel.AutoSize = true;
            totalDueTextLabel.Dock = DockStyle.Top;
            totalDueTextLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            totalDueTextLabel.Location = new Point(30, 296);
            totalDueTextLabel.Margin = new Padding(4, 0, 4, 0);
            totalDueTextLabel.Name = "totalDueTextLabel";
            totalDueTextLabel.Size = new Size(194, 45);
            totalDueTextLabel.TabIndex = 2;
            totalDueTextLabel.Text = "TOTAL DUE";
            // 
            // separatorPanel
            // 
            separatorPanel.BackColor = SystemColors.ControlDark;
            separatorPanel.Dock = DockStyle.Top;
            separatorPanel.Location = new Point(30, 294);
            separatorPanel.Margin = new Padding(4, 34, 4, 34);
            separatorPanel.Name = "separatorPanel";
            separatorPanel.Size = new Size(573, 2);
            separatorPanel.TabIndex = 3;
            // 
            // discountFlowLayoutPanel
            // 
            discountFlowLayoutPanel.AutoSize = true;
            discountFlowLayoutPanel.Controls.Add(discountTypeComboBox);
            discountFlowLayoutPanel.Controls.Add(discountNumericUpDown);
            discountFlowLayoutPanel.Controls.Add(applyDiscountButton);
            discountFlowLayoutPanel.Dock = DockStyle.Top;
            discountFlowLayoutPanel.Location = new Point(30, 242);
            discountFlowLayoutPanel.Margin = new Padding(4);
            discountFlowLayoutPanel.Name = "discountFlowLayoutPanel";
            discountFlowLayoutPanel.Size = new Size(573, 52);
            discountFlowLayoutPanel.TabIndex = 4;
            // 
            // discountTypeComboBox
            // 
            discountTypeComboBox.FormattingEnabled = true;
            discountTypeComboBox.Location = new Point(4, 4);
            discountTypeComboBox.Margin = new Padding(4);
            discountTypeComboBox.Name = "discountTypeComboBox";
            discountTypeComboBox.Size = new Size(180, 38);
            discountTypeComboBox.TabIndex = 0;
            // 
            // discountNumericUpDown
            // 
            discountNumericUpDown.Location = new Point(192, 4);
            discountNumericUpDown.Margin = new Padding(4);
            discountNumericUpDown.Name = "discountNumericUpDown";
            discountNumericUpDown.Size = new Size(180, 35);
            discountNumericUpDown.TabIndex = 1;
            // 
            // applyDiscountButton
            // 
            applyDiscountButton.Location = new Point(380, 4);
            applyDiscountButton.Margin = new Padding(4);
            applyDiscountButton.Name = "applyDiscountButton";
            applyDiscountButton.Size = new Size(180, 44);
            applyDiscountButton.TabIndex = 2;
            applyDiscountButton.Text = "Apply Discount";
            // 
            // totalsTableLayoutPanel
            // 
            totalsTableLayoutPanel.ColumnCount = 2;
            totalsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            totalsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            totalsTableLayoutPanel.Controls.Add(subtotalTextLabel, 0, 0);
            totalsTableLayoutPanel.Controls.Add(subtotalValueLabel, 1, 0);
            totalsTableLayoutPanel.Controls.Add(discountTextLabel, 0, 1);
            totalsTableLayoutPanel.Controls.Add(discountValueLabel, 1, 1);
            totalsTableLayoutPanel.Dock = DockStyle.Top;
            totalsTableLayoutPanel.Location = new Point(30, 34);
            totalsTableLayoutPanel.Margin = new Padding(4);
            totalsTableLayoutPanel.Name = "totalsTableLayoutPanel";
            totalsTableLayoutPanel.RowCount = 2;
            totalsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            totalsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            totalsTableLayoutPanel.Size = new Size(573, 208);
            totalsTableLayoutPanel.TabIndex = 5;
            // 
            // subtotalTextLabel
            // 
            subtotalTextLabel.Dock = DockStyle.Fill;
            subtotalTextLabel.Font = new Font("Segoe UI", 12F);
            subtotalTextLabel.Location = new Point(4, 0);
            subtotalTextLabel.Margin = new Padding(4, 0, 4, 0);
            subtotalTextLabel.Name = "subtotalTextLabel";
            subtotalTextLabel.Size = new Size(278, 104);
            subtotalTextLabel.TabIndex = 0;
            subtotalTextLabel.Text = "Subtotal";
            subtotalTextLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // subtotalValueLabel
            // 
            subtotalValueLabel.Dock = DockStyle.Fill;
            subtotalValueLabel.Font = new Font("Segoe UI", 12F);
            subtotalValueLabel.Location = new Point(290, 0);
            subtotalValueLabel.Margin = new Padding(4, 0, 4, 0);
            subtotalValueLabel.Name = "subtotalValueLabel";
            subtotalValueLabel.Size = new Size(279, 104);
            subtotalValueLabel.TabIndex = 1;
            subtotalValueLabel.Text = "$4.25";
            subtotalValueLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // discountTextLabel
            // 
            discountTextLabel.Dock = DockStyle.Fill;
            discountTextLabel.Font = new Font("Segoe UI", 12F);
            discountTextLabel.Location = new Point(4, 104);
            discountTextLabel.Margin = new Padding(4, 0, 4, 0);
            discountTextLabel.Name = "discountTextLabel";
            discountTextLabel.Size = new Size(278, 104);
            discountTextLabel.TabIndex = 2;
            discountTextLabel.Text = "Discount";
            discountTextLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // discountValueLabel
            // 
            discountValueLabel.Dock = DockStyle.Fill;
            discountValueLabel.Font = new Font("Segoe UI", 12F);
            discountValueLabel.Location = new Point(290, 104);
            discountValueLabel.Margin = new Padding(4, 0, 4, 0);
            discountValueLabel.Name = "discountValueLabel";
            discountValueLabel.Size = new Size(279, 104);
            discountValueLabel.TabIndex = 3;
            discountValueLabel.Text = "$0.00";
            discountValueLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CashierPOSForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2880, 1588);
            Controls.Add(mainContentPanel);
            Controls.Add(subHeaderPanel);
            Controls.Add(headerPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "CashierPOSForm";
            Text = "BIMS Point of Sale";
            WindowState = FormWindowState.Maximized;
            headerPanel.ResumeLayout(false);
            welcomeFlowLayoutPanel.ResumeLayout(false);
            welcomeFlowLayoutPanel.PerformLayout();
            subHeaderPanel.ResumeLayout(false);
            subHeaderButtonPanel.ResumeLayout(false);
            mainContentPanel.ResumeLayout(false);
            mainTableLayoutPanel.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            addItemContainerPanel.ResumeLayout(false);
            addItemContainerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)quantityNumericUpDown).EndInit();
            rightPanel.ResumeLayout(false);
            billAndTotalsSplitter.ResumeLayout(false);
            billItemsSectionPanel.ResumeLayout(false);
            billItemsSectionPanel.PerformLayout();
            totalsSectionPanel.ResumeLayout(false);
            totalsSectionPanel.PerformLayout();
            discountFlowLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)discountNumericUpDown).EndInit();
            totalsTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label bimsLabel;
        private System.Windows.Forms.FlowLayoutPanel welcomeFlowLayoutPanel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Button userProfileButton;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Panel mainContentPanel;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel addItemContainerPanel;
        private System.Windows.Forms.Label scanLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ListBox searchResultsListBox;
        private System.Windows.Forms.Label quantityLabel;
        private System.Windows.Forms.NumericUpDown quantityNumericUpDown;
        private System.Windows.Forms.Button addToBillButton;
        private System.Windows.Forms.TableLayoutPanel billAndTotalsSplitter;
        private System.Windows.Forms.Panel billItemsSectionPanel;
        private System.Windows.Forms.Panel totalsSectionPanel;
        private System.Windows.Forms.FlowLayoutPanel billItemsFlowPanel;
        private System.Windows.Forms.Label currentBillLabel;
        private System.Windows.Forms.TableLayoutPanel totalsTableLayoutPanel;
        private System.Windows.Forms.Label subtotalTextLabel;
        private System.Windows.Forms.Label subtotalValueLabel;
        private System.Windows.Forms.Label discountTextLabel;
        private System.Windows.Forms.Label discountValueLabel;
        private System.Windows.Forms.Panel separatorPanel;
        private System.Windows.Forms.Label totalDueTextLabel;
        private System.Windows.Forms.Label totalDueValueLabel;
        private System.Windows.Forms.Button finalizeAndPayButton;
        private System.Windows.Forms.Panel subHeaderPanel;
        private System.Windows.Forms.Label invoiceLabel;
        private System.Windows.Forms.FlowLayoutPanel subHeaderButtonPanel;
        private System.Windows.Forms.Button cancelTransactionButton;
        private System.Windows.Forms.Button removeItemButton;
        private System.Windows.Forms.FlowLayoutPanel discountFlowLayoutPanel;
        private System.Windows.Forms.ComboBox discountTypeComboBox;
        private System.Windows.Forms.NumericUpDown discountNumericUpDown;
        private System.Windows.Forms.Button applyDiscountButton;
    }
}