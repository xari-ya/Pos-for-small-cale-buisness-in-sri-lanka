using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;

namespace billing_system
{
    /// <summary>
    /// Represents a form for adding a new product or editing an existing one.
    /// </summary>
    /// <remarks>
    /// This form is used by the <see cref="InventoryControl"/> to manage product details. It operates in two modes:
    /// 'Add' for creating new products and 'Edit' for modifying existing ones. The mode is determined by the
    /// constructor that is used to create the form.
    /// </remarks>
    public partial class ProductDetailForm : Form
    {
        /// <summary>
        /// Represents the data collected from the form's input fields.
        /// </summary>
        /// <remarks>
        /// An instance of this class is returned to the caller via the <see cref="Result"/> property when the form is submitted.
        /// </remarks>
        public sealed class ProductInput
        {
            /// <summary>Gets or sets the product's SKU.</summary>
            public string Sku { get; set; } = "";
            /// <summary>Gets or sets the product's name.</summary>
            public string Name { get; set; } = "";
            /// <summary>Gets or sets the product's category ID.</summary>
            public string? CategoryId { get; set; }
            /// <summary>Gets or sets the product's unit price.</summary>
            public decimal UnitPrice { get; set; }
            /// <summary>Gets or sets the product's opening quantity (for 'Add' mode only).</summary>
            public decimal OpeningQty { get; set; }
        }

        private readonly bool _isEdit;
        private readonly long _productId;
        private readonly string? _initialCategoryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailForm"/> class for adding a new product.
        /// </summary>
        public ProductDetailForm()
        {
            InitializeComponent();
            _isEdit = false;
            _productId = 0;
            _initialCategoryId = null;
            this.lblHeader.Text = "Add Product";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailForm"/> class for editing an existing product.
        /// </summary>
        /// <param name="productId">The ID of the product to edit.</param>
        /// <param name="sku">The current SKU of the product.</param>
        /// <param name="name">The current name of the product.</param>
        /// <param name="categoryId">The current category ID of the product.</param>
        /// <param name="price">The current price of the product.</param>
        public ProductDetailForm(long productId, string sku, string name, string? categoryId, decimal price)
        {
            InitializeComponent();
            _isEdit = true;
            _productId = productId;
            _initialCategoryId = categoryId;
            this.lblHeader.Text = "Edit Product";
            txtSku.Text = sku;
            txtName.Text = name;
            numPrice.Value = price;
            numOpeningQty.Enabled = false;
        }

        /// <summary>
        /// Gets the result of the user's input.
        /// </summary>
        /// <remarks>
        /// This property is populated with the form's data when the user clicks the "Save" button.
        /// </remarks>
        public ProductInput Result { get; private set; } = new ProductInput();

        /// <summary>
        /// Handles the Load event of the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method populates the category combo box by fetching data from the <see cref="CategoryRepository"/>.
        /// In 'Edit' mode, it pre-selects the product's current category.
        /// </remarks>
        private void ProductDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                var cats = CategoryRepository.GetAll(conn);
                cats.Insert(0, new CategoryRepository.CategoryItem { CategoryId = "", Name = "(None)" });
                cbCategory.DataSource = cats;
                cbCategory.DisplayMember = "Name";
                cbCategory.ValueMember = "CategoryId";
                if (_isEdit)
                {
                    var idx = cats.FindIndex(c => c.CategoryId == (_initialCategoryId ?? ""));
                    cbCategory.SelectedIndex = idx >= 0 ? idx : 0;
                    txtSku.Focus();
                }
                else
                {
                    cbCategory.SelectedIndex = 0;
                    txtSku.Focus();
                }
                Console.WriteLine("[ProductDetailForm] Categories loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ProductDetailForm] ERROR loading categories: " + ex);
            }
        }

        /// <summary>
        /// Handles the Click event for the "Save" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method validates the user's input, populates the <see cref="Result"/> property, sets the form's
        /// <see cref="DialogResult"/> to <see cref="DialogResult.OK"/>, and closes the form.
        /// </remarks>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string sku = (txtSku.Text ?? "").Trim();
            string name = (txtName.Text ?? "").Trim();
            decimal price = numPrice.Value;
            string? categoryId = (cbCategory.SelectedValue?.ToString() ?? "");
            if (string.IsNullOrEmpty(categoryId)) categoryId = null;
            decimal openingQty = numOpeningQty.Enabled ? numOpeningQty.Value : 0m;
            if (string.IsNullOrWhiteSpace(sku))
            {
                Console.WriteLine("[ProductDetailForm] Validation failed: SKU is required.");
                txtSku.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("[ProductDetailForm] Validation failed: Name is required.");
                txtName.Focus();
                return;
            }
            if (price < 0)
            {
                Console.WriteLine("[ProductDetailForm] Validation failed: Price < 0.");
                numPrice.Focus();
                return;
            }
            if (!_isEdit && openingQty < 0)
            {
                Console.WriteLine("[ProductDetailForm] Validation failed: OpeningQty < 0.");
                numOpeningQty.Focus();
                return;
            }
            Result = new ProductInput { Sku = sku, Name = name, CategoryId = categoryId, UnitPrice = price, OpeningQty = openingQty };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event for the "Cancel" button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        /// <remarks>
        /// This method sets the form's <see cref="DialogResult"/> to <see cref="DialogResult.Cancel"/> and closes the form.
        /// </remarks>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
