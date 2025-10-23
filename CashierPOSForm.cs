// File: CashierPOSForm.cs
using billing_system;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace billing_system
{
    public partial class CashierPOSForm : Form
    {
        private TransactionManager transactionManager;
        private List<Product> currentSearchResults;

        public CashierPOSForm()
        {
            InitializeComponent();
            // Assuming AppSession.CurrentUser is set at login. For testing:
            if (AppSession.CurrentUser == null)
            {
                AppSession.CurrentUser = new User { UserId = 1, FullName = "Default Cashier" };
            }
            WireEvents();
            InitializeDiscountControls(); // NEW: Setup our new controls
        }

        private void WireEvents()
        {
            this.Load += CashierPOSForm_Load;
            this.searchTextBox.TextChanged += searchTextBox_TextChanged;
            this.searchResultsListBox.DoubleClick += searchResultsListBox_DoubleClick;
            this.addToBillButton.Click += addToBillButton_Click;
            this.removeItemButton.Click += removeItemButton_Click;
            this.cancelTransactionButton.Click += cancelTransactionButton_Click;
            this.applyDiscountButton.Click += applyDiscountButton_Click; // NEW: Wire up the new button
            this.logoutButton.Click += logoutButton_Click;

        }

        private void InitializeDiscountControls()
        {
            // Populate the dropdown with our enum types
            discountTypeComboBox.DataSource = Enum.GetValues(typeof(DiscountType));
            discountTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // Set properties for the number input
            discountNumericUpDown.DecimalPlaces = 2;
            discountNumericUpDown.Maximum = 100000;
        }


        private void CashierPOSForm_Load(object sender, EventArgs e)
        {
            AuthTroubleshooter.CheckCurrentUser("On POS Form Load");
            welcomeLabel.Text = $"Welcome, {AppSession.CurrentUser?.FullName ?? "Cashier"}"; // <-- Ensure this runs early

            AuthTroubleshooter.CheckDatabase();

            StartNewTransaction();
        }

        private void StartNewTransaction()
        {
            transactionManager = new TransactionManager();
            RefreshUI();
        }

        private void applyDiscountButton_Click(object sender, EventArgs e)
        {
            // Read values from the new UI controls
            decimal discountValue = discountNumericUpDown.Value;
            DiscountType type = (DiscountType)discountTypeComboBox.SelectedItem;

            // Tell the manager to apply the discount
            transactionManager.ApplyDiscount(discountValue, type);

            // Refresh the totals on the screen
            UpdateTotalsDisplay();
        }


        // --- EVENT HANDLERS ---

        // Keeps track of which item was double-clicked last
        private BillItem _lastClickedBillItem;

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchResultsListBox.Visible = false;
                return;
            }

            currentSearchResults = transactionManager.SearchProducts(searchText);
            searchResultsListBox.DataSource = currentSearchResults;
            searchResultsListBox.DisplayMember = "Name";
            searchResultsListBox.ValueMember = "Id";
            searchResultsListBox.Visible = true;
        }

        private void searchResultsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (searchResultsListBox.SelectedItem is Product selectedProduct)
            {
                searchTextBox.Text = selectedProduct.Name;
                searchResultsListBox.Visible = false;
                quantityNumericUpDown.Focus();
            }
        }

        private void addToBillButton_Click(object sender, EventArgs e)
        {
            // Find the selected product from the last search result to ensure we have the full object
            var selectedProduct = currentSearchResults?.Find(p => p.Name == searchTextBox.Text);

            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a valid product from the list.", "Invalid Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = (int)quantityNumericUpDown.Value;
            transactionManager.AddItemToBill(selectedProduct, quantity);

            // Reset input fields for next item
            searchTextBox.Clear();
            quantityNumericUpDown.Value = 1;
            searchTextBox.Focus();

            RefreshUI();
        }

        private void removeItemButton_Click(object sender, EventArgs e)
        {
            if (_lastClickedBillItem == null)
            {
                MessageBox.Show("Double-click an item in the bill to select it first.",
                                "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to remove {_lastClickedBillItem.ProductDetails.Name}?",
                "Confirm Removal",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                transactionManager.RemoveItemFromBill(_lastClickedBillItem);
                _lastClickedBillItem = null;
                RefreshUI();
            }
        }


        private void cancelTransactionButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to cancel this entire transaction?",
                                         "Confirm Cancellation",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                transactionManager.ClearTransaction();
                RefreshUI();
            }
        }

        private void finalizeAndPayButton_Click_1(object sender, EventArgs e)
        {
            if (transactionManager.CurrentBillItems.Count == 0)
            {
                MessageBox.Show("Cannot process payment for an empty bill.", "Empty Bill", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create the payment form and PASS the transaction manager to it
            using (var paymentForm = new PaymentForm(this.transactionManager))
            {
                // Show the form as a dialog, which pauses this form
                var result = paymentForm.ShowDialog();

                // After the payment form closes, check if the payment was successful
                if (result == DialogResult.OK)
                {
                    // If it was, reset the screen for the next sale
                    StartNewTransaction();
                }
                // If the user just closed the payment form, do nothing.
            }
        }

        // --- UI HELPER METHODS ---

        private void RefreshUI()
        {
            RefreshBillDisplay();
            UpdateTotalsDisplay();
            invoiceLabel.Text = $"Invoice #: {transactionManager.InvoiceNumber}";
        }
        private void BillRow_DoubleClick(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl == null) return;

            // Try to get the BillItem from the control or its parent panel
            var item = ctrl.Tag as BillItem
                       ?? (ctrl.Parent as Panel)?.Tag as BillItem;
            if (item == null) return;

            var confirm = MessageBox.Show(
                $"Remove {item.ProductDetails.Name}?",
                "Confirm Removal",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                transactionManager.RemoveItemFromBill(item);
                RefreshUI();
            }
        }


        /// <summary>
        /// Clears and rebuilds the bill items panel from the TransactionManager's list.
        /// </summary>
        private void RefreshBillDisplay()
        {
            var sriLankanCulture = new CultureInfo("en-LK");
            sriLankanCulture.NumberFormat.CurrencySymbol = "Rs.";
            sriLankanCulture.NumberFormat.CurrencyPositivePattern = 2;
            billItemsFlowPanel.Controls.Clear();

            foreach (var item in transactionManager.CurrentBillItems)
            {
                var itemPanel = new Panel
                {
                    // --- THE FIX IS HERE ---
                    // Removed the "- 25" so the panel takes the full available width.
                    Width = billItemsFlowPanel.ClientSize.Width - 20,
                    Height = 100,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(3),
                    BackColor = Color.White,
                    Tag = item
                };

                var nameLabel = new Label
                {
                    Text = item.ProductDetails.Name,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(10, 15),
                    AutoSize = true,
                    BackColor = Color.White,
                    Tag = item
                };

                var quantityLabel = new Label
                {
                    Text = $"{item.Quantity} x {item.UnitPriceAtSale.ToString("C2", sriLankanCulture)}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(10, 55),
                    AutoSize = true,
                    BackColor = Color.White,
                    Tag = item
                };

                var totalLabel = new Label
                {
                    Text = item.LineTotal.ToString("C2", sriLankanCulture),
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Dock = DockStyle.Right,
                    TextAlign = ContentAlignment.MiddleRight,
                    Width = 200,
                    BackColor = Color.White,
                    Tag = item
                };

                itemPanel.DoubleClick += BillRow_DoubleClick;
                nameLabel.DoubleClick += BillRow_DoubleClick;
                quantityLabel.DoubleClick += BillRow_DoubleClick;
                totalLabel.DoubleClick += BillRow_DoubleClick;

                itemPanel.Controls.Add(nameLabel);
                itemPanel.Controls.Add(quantityLabel);
                itemPanel.Controls.Add(totalLabel);
                billItemsFlowPanel.Controls.Add(itemPanel);
            }
        }


        /// <summary>
        /// Updates all total labels from the TransactionManager's properties.
        /// </summary>
        private void UpdateTotalsDisplay()
        {
            var sriLankanCulture = new CultureInfo("en-LK");
            sriLankanCulture.NumberFormat.CurrencySymbol = "Rs.";
            sriLankanCulture.NumberFormat.CurrencyPositivePattern = 2;
            subtotalValueLabel.Text = transactionManager.Subtotal.ToString("C2", sriLankanCulture);
            discountValueLabel.Text = transactionManager.DiscountTotal.ToString("C2", sriLankanCulture);
            totalDueValueLabel.Text = transactionManager.GrandTotal.ToString("C2", sriLankanCulture);
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // Perform logout
                var authService = new AuthService();
                authService.Logout();

                // Close the POS form
                this.Hide();

                // Open the login form again
                var loginForm = new Login();
                loginForm.Show();

                // Close this form once login form is shown
            }
        }

    }
}