namespace Ecommerce_Webservices.RequestDateObject
{
    public class ProductReq
    {
       
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } = 0;
        public string Brand { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class ProductUpdateReq:ProductReq
    {

        public string ProductId { get; set; } = string.Empty ;
    }
}
