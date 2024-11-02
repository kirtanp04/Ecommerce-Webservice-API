using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webservices.DataObject
{
    public class Register
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty ;

    }
}
