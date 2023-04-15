using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Idea1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Idea1.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Idea1.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Idea1.Models.Idea> Ideas { get; set; }

        public System.Data.Entity.DbSet<Idea1.Models.Topic> Topics { get; set; }

        public System.Data.Entity.DbSet<Idea1.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<Idea1.Models.Stastus> Stastus { get; set; }

    }
}