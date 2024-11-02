namespace Ecommerce_Webservices.DataObject
{
    public enum enumShippingMethod
    {
        Standard,
        Express,
        Overnight,
        International,
        Pickup
    }

    public class Shipping
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string ShippingAddressId {  get; set; } = string.Empty;
        public enumShippingMethod Method { get; set; }

        public string TrackingNumber { get; set; } = string.Empty;
        public DateTime ShippedDate {  get; set; }
        public DateTime EstimateDeliveryDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }

    }
}
