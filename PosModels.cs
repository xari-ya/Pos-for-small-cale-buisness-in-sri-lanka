// In new file: PosModels.cs

namespace billing_system
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }

    public class BillItem
    {
        public Product ProductDetails { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRatePercent { get; set; }

        public decimal LineTotal => (UnitPriceAtSale * Quantity) - DiscountAmount;
    }
}