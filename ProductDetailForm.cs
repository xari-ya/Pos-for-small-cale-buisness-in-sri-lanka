// -----------------------------------------------------------------------------
// ProductDetailForm.cs (EXPLAINED)
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace billing_system
{
    public partial class ProductDetailForm : System.Windows.Forms.Form
    {
        // Model returned to the caller on OK
        public sealed class ProductInput
        {
            public string Sku { get; set; } = "";
            public string Name { get; set; } = "";
            public string? CategoryId { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal OpeningQty { get; set; }   // for add only; ignored on edit
        }

        private readonly bool _isEdit;
        private readonly long _productId;            // only used in edit
        private readonly string? _initialCategoryId;

        // Add-new constructor
        public ProductDetailForm()
        {
            InitializeComponent();
            _isEdit = false;
            _productId = 0;
            _initialCategoryId = null;
            this.lblHeader.Text = "Add Product";
        }

        // Edit constructor
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
            numOpeningQty.Enabled = false;   // no opening stock during edit
        }

        public ProductInput Result { get; private set; } = new ProductInput();

        private void ProductDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                var cats = CategoryRepository.GetAll(conn);
                // Insert blank/none option at top
                cats.Insert(0, new CategoryRepository.CategoryItem { CategoryId = "", Name = "(None)" });

                cbCategory.DataSource = cats;
                cbCategory.DisplayMember = "Name";
                cbCategory.ValueMember = "CategoryId";

                if (_isEdit)
                {
                    // Pre-select existing category
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate minimal rules — print to terminal only
            string sku = (txtSku.Text ?? "").Trim();
            string name = (txtName.Text ?? "").Trim();
            decimal price = numPrice.Value;
            string? categoryId = (cbCategory.SelectedValue?.ToString() ?? "");
            if (string.IsNullOrEmpty(categoryId)) categoryId = null; // treat (None) as null
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

            Result = new ProductInput
            {
                Sku = sku,
                Name = name,
                CategoryId = categoryId,
                UnitPrice = price,
                OpeningQty = openingQty
            };

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
