namespace Ecommerce_Webservices.DataObject
{
    public enum enumPaymentMethod
    {
        Credit_Card,
        UPI
    }

    public enum enumPaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public class Payments
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public enumPaymentMethod Paymentmethod { get; set; }
        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public enumPaymentStatus Status { get; set; }
    }
}
