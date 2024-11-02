using Microsoft.AspNetCore.Identity;

namespace Ecommerce_Webservices.DataObject
{
    public class User:IdentityUser
    {
      public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
      public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
      public bool IsActive { get; set; } = false;
      public bool IsPrimeMember { get; set; } = false ;
    }
}
