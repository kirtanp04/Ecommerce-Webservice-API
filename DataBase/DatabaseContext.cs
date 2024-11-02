using Ecommerce_Webservices.DataObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webservices.DataBase
{
    public class DatabaseContext : IdentityDbContext<User, RoleInfo, string, IdentityUserClaim<string>, RoleAssignInfo, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<Review> Review { get; set; }
    }
}
