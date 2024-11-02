namespace Ecommerce_Webservices.DataObject
{
    public class UserResponse<T>
    {
        public bool isSuccess {  get; set; }
        public string message { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}
