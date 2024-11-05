

using System.Security.Claims;

namespace Ecommerce_Webservices.Common
{
    public class CommonVar
    {
        public static string NoPermission = "You don't have the permission to access this service.";
        public static string InvalidUser = "Invalid user info, there is no such user found.";
        public static string getUserIdfromClaims(ClaimsPrincipal User)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "userID")?.Value!;

            if(userId is null )
            {
                return "";
            }

            return userId;
        }
    }
}
