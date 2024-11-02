namespace Ecommerce_Webservices.DataObject
{
    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Brand { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal Rating { get; set; } 
        public int TotalReviews {  get; set; }
        public string Dimensions {  get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public bool IsActive { get; set; } = false;

    }
}
