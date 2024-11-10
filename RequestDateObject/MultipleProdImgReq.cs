namespace Ecommerce_Webservices.RequestDateObject
{
    public class MultipleProdImgReq
    {
        public string ProductId { get; set; } = string.Empty;
        public List<Img> arrImg { get; set; }
    }

    public class Img
    {
        public string Id { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}
