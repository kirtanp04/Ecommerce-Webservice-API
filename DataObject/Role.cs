using Microsoft.AspNetCore.Identity;

namespace Ecommerce_Webservices.DataObject
{
    public class RoleInfo:IdentityRole
    {

       public string CreatedBy { get; set; } = string.Empty;

       public DateTime CreatedAt { get; set; }

    }

    public class RoleAssignInfo : IdentityUserRole<string>
    {
        public string AssignedBy { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}
