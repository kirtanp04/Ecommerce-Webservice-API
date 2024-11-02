namespace Ecommerce_Webservices.DataObject
{
    public class ProductVariant
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string Size {  get; set; } = string.Empty;
        public string Color {  get; set; } = string.Empty;
        public decimal AdditionalPrice {  get; set; } 
    }
}
