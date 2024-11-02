namespace Ecommerce_Webservices.DataObject
{
    public class OrderItems
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty ;
        public string ProductId {  get; set; } = string.Empty ;
        public string VariantId {  get; set; } = string.Empty ;
        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }
    }
}
