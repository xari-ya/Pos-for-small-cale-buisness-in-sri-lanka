namespace billing_system
{
    /// <summary>
    /// Represents a product in the point-of-sale system.
    /// </summary>
    /// <remarks>
    /// This class is a simplified data model used for product searches and within the billing process.
    /// </remarks>
    public class Product
    {
        /// <summary>Gets or sets the unique identifier for the product.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the stock keeping unit (SKU) of the product.</summary>
        public string Sku { get; set; }
        /// <summary>Gets or sets the name of the product.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets the price of the product.</summary>
        public decimal Price { get; set; }
        /// <summary>Gets or sets a value indicating whether the product is active.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Represents an item on a customer's bill.
    /// </summary>
    /// <remarks>
    /// This class holds the details of a single product being sold, including the quantity, the price at the time of sale,
    /// and any applicable discounts or taxes.
    /// </remarks>
    public class BillItem
    {
        /// <summary>Gets or sets the product details for this bill item.</summary>
        public Product ProductDetails { get; set; }
        /// <summary>Gets or sets the quantity of the product being sold.</summary>
        public int Quantity { get; set; }
        /// <summary>Gets or sets the unit price of the product at the time of sale.</summary>
        public decimal UnitPriceAtSale { get; set; }
        /// <summary>Gets or sets the total discount amount applied to this line item.</summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>Gets or sets the tax rate percentage for this line item.</summary>
        public decimal TaxRatePercent { get; set; }

        /// <summary>
        /// Calculates the total price for this line item, accounting for quantity and discounts.
        /// </summary>
        public decimal LineTotal => (UnitPriceAtSale * Quantity) - DiscountAmount;
    }
}
