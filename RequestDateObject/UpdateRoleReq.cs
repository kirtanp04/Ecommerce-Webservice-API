namespace Ecommerce_Webservices.RequestDateObject
{
    public class UpdateRoleReq
    {
        public string RoleId { get; set; } = string.Empty;
        public string NewRoleName { get; set; } = string.Empty;

        public string UpdateByUser {  get; set; } = string.Empty;
    }
}
