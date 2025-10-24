using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents the main Point of Sale (POS) form for cashiers.
    /// </summary>
    /// <remarks>
    /// This form provides the primary interface for cashiers to create and manage customer transactions.
    /// It handles product searching, adding items to a bill, applying discounts, and initiating the payment process.
    /// The core logic is managed by a <see cref="TransactionManager"/> instance.
    /// </remarks>
    public partial class CashierPOSForm : Form
    {
        private TransactionManager transactionManager;
        private List<Product> currentSearchResults;
        private BillItem _lastClickedBillItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CashierPOSForm"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes form components, sets up event handlers, and configures the discount controls.
        /// It also ensures that a user is logged in via <see cref="AppSession.CurrentUser"/>.
        /// </remarks>
        public CashierPOSForm()
        {
            InitializeComponent();
            if (AppSession.CurrentUser == null)
            {
                AppSession.CurrentUser = new User { UserId = 1, FullName = "Default Cashier" };
            }
            WireEvents();
            InitializeDiscountControls();
        }

        /// <summary>
        /// Wires up all the necessary event handlers for the form's controls.
        /// </summary>
        private void WireEvents()
        {
            this.Load += CashierPOSForm_Load;
            this.searchTextBox.TextChanged += searchTextBox_TextChanged;
            this.searchResultsListBox.DoubleClick += searchResultsListBox_DoubleClick;
            this.addToBillButton.Click += addToBillButton_Click;
            this.removeItemButton.Click += removeItemButton_Click;
            this.cancelTransactionButton.Click += cancelTransactionButton_Click;
            this.applyDiscountButton.Click += applyDiscountButton_Click;
            this.logoutButton.Click += logoutButton_Click;
        }

        /// <summary>
        /// Initializes the discount-related UI controls.
        /// </summary>
        /// <remarks>
        /// This method populates the discount type combo box with values from the <see cref="DiscountType"/> enum
        /// and configures the numeric up-down control for discount values.
        /// </remarks>
        private void InitializeDiscountControls()
        {
            discountTypeComboBox.DataSource = Enum.GetValues(typeof(DiscountType));
            discountTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            discountNumericUpDown.DecimalPlaces = 2;
            discountNumericUpDown.Maximum = 100000;
        }

        /// <summary>
        /// Handles the Load event of the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method sets the welcome message, runs diagnostic checks using <see cref="AuthTroubleshooter"/>,
        /// and initializes a new transaction.
        /// </remarks>
        private void CashierPOSForm_Load(object sender, EventArgs e)
        {
            AuthTroubleshooter.CheckCurrentUser("On POS Form Load");
            welcomeLabel.Text = $"Welcome, {AppSession.CurrentUser?.FullName ?? "Cashier"}";
            AuthTroubleshooter.CheckDatabase();
            StartNewTransaction();
        }

        /// <summary>
        /// Starts a new transaction by creating a new <see cref="TransactionManager"/> instance and refreshing the UI.
        /// </summary>
        private void StartNewTransaction()
        {
            transactionManager = new TransactionManager();
            RefreshUI();
        }

        /// <summary>
        /// Handles the Click event for the "Apply Discount" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method reads the discount value and type from the UI controls and applies it to the current transaction
        /// via the <see cref="TransactionManager"/>. It then updates the totals display.
        /// </remarks>
        private void applyDiscountButton_Click(object sender, EventArgs e)
        {
            decimal discountValue = discountNumericUpDown.Value;
            DiscountType type = (DiscountType)discountTypeComboBox.SelectedItem;
            transactionManager.ApplyDiscount(discountValue, type);
            UpdateTotalsDisplay();
        }

        /// <summary>
        /// Handles the TextChanged event for the search text box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method searches for products based on the input text and displays the results in a list box.
        /// </remarks>
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

        /// <summary>
        /// Handles the DoubleClick event for the search results list box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method auto-fills the search box with the selected product's name and hides the list box.
        /// </remarks>
        private void searchResultsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (searchResultsListBox.SelectedItem is Product selectedProduct)
            {
                searchTextBox.Text = selectedProduct.Name;
                searchResultsListBox.Visible = false;
                quantityNumericUpDown.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event for the "Add to Bill" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method adds the selected product and quantity to the current bill via the <see cref="TransactionManager"/>
        /// and then refreshes the UI.
        /// </remarks>
        private void addToBillButton_Click(object sender, EventArgs e)
        {
            var selectedProduct = currentSearchResults?.Find(p => p.Name == searchTextBox.Text);
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a valid product from the list.", "Invalid Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int quantity = (int)quantityNumericUpDown.Value;
            transactionManager.AddItemToBill(selectedProduct, quantity);
            searchTextBox.Clear();
            quantityNumericUpDown.Value = 1;
            searchTextBox.Focus();
            RefreshUI();
        }

        /// <summary>
        /// Handles the Click event for the "Remove Item" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method removes the last double-clicked item from the bill after user confirmation.
        /// </remarks>
        private void removeItemButton_Click(object sender, EventArgs e)
        {
            if (_lastClickedBillItem == null)
            {
                MessageBox.Show("Double-click an item in the bill to select it first.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var confirm = MessageBox.Show($"Are you sure you want to remove {_lastClickedBillItem.ProductDetails.Name}?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                transactionManager.RemoveItemFromBill(_lastClickedBillItem);
                _lastClickedBillItem = null;
                RefreshUI();
            }
        }

        /// <summary>
        /// Handles the Click event for the "Cancel Transaction" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method clears the entire transaction after user confirmation.
        /// </remarks>
        private void cancelTransactionButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to cancel this entire transaction?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                transactionManager.ClearTransaction();
                RefreshUI();
            }
        }

        /// <summary>
        /// Handles the Click event for the "Finalize and Pay" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method opens the <see cref="PaymentForm"/> as a dialog, passing the current <see cref="TransactionManager"/> to it.
        /// If the payment is successful, it starts a new transaction.
        /// </remarks>
        private void finalizeAndPayButton_Click_1(object sender, EventArgs e)
        {
            if (transactionManager.CurrentBillItems.Count == 0)
            {
                MessageBox.Show("Cannot process payment for an empty bill.", "Empty Bill", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var paymentForm = new PaymentForm(this.transactionManager))
            {
                var result = paymentForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StartNewTransaction();
                }
            }
        }

        /// <summary>
        /// Refreshes the entire UI by updating the bill display, totals, and invoice number.
        /// </summary>
        private void RefreshUI()
        {
            RefreshBillDisplay();
            UpdateTotalsDisplay();
            invoiceLabel.Text = $"Invoice #: {transactionManager.InvoiceNumber}";
        }

        /// <summary>
        /// Handles the DoubleClick event for a dynamically created bill item row.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method prompts the user to confirm the removal of the double-clicked item from the bill.
        /// </remarks>
        private void BillRow_DoubleClick(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl == null) return;
            var item = ctrl.Tag as BillItem ?? (ctrl.Parent as Panel)?.Tag as BillItem;
            if (item == null) return;
            var confirm = MessageBox.Show($"Remove {item.ProductDetails.Name}?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                transactionManager.RemoveItemFromBill(item);
                RefreshUI();
            }
        }

        /// <summary>
        /// Clears and rebuilds the bill items panel from the TransactionManager's list.
        /// </summary>
        /// <remarks>
        /// This method dynamically creates a panel with labels for each item in the bill, displaying product name,
        /// quantity, price, and total. It also attaches a double-click event handler for item removal.
        /// </remarks>
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
        /// <remarks>
        /// This method formats the subtotal, discount, and grand total as currency and displays them in their respective labels.
        /// </remarks>
        private void UpdateTotalsDisplay()
        {
            var sriLankanCulture = new CultureInfo("en-LK");
            sriLankanCulture.NumberFormat.CurrencySymbol = "Rs.";
            sriLankanCulture.NumberFormat.CurrencyPositivePattern = 2;
            subtotalValueLabel.Text = transactionManager.Subtotal.ToString("C2", sriLankanCulture);
            discountValueLabel.Text = transactionManager.DiscountTotal.ToString("C2", sriLankanCulture);
            totalDueValueLabel.Text = transactionManager.GrandTotal.ToString("C2", sriLankanCulture);
        }

        /// <summary>
        /// Handles the Click event for the Logout button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method prompts the user for confirmation, and if confirmed, logs out the current user via the
        /// <see cref="AuthService"/> and displays the <see cref="Login"/> form.
        /// </remarks>
        private void logoutButton_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var authService = new AuthService();
                authService.Logout();
                this.Hide();
                var loginForm = new Login();
                loginForm.Show();
            }
        }
    }
}
