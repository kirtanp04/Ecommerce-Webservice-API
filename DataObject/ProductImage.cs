namespace Ecommerce_Webservices.DataObject
{
    public class ProductImage
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string Url {  get; set; } = string.Empty;
        public string AltText {  get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}
