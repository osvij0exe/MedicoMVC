using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicoMVC.DataAcces
{
    public class MedicosApplicationDBContext : IdentityDbContext
    {
        public MedicosApplicationDBContext(DbContextOptions<MedicosApplicationDBContext> options)
            : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //AspNetUsers
            builder.Entity<MedicoIdentityUser>(e =>
            {
                e.ToTable("Usuario");
            });

            //AspNetRoles
            builder.Entity<IdentityRole>(r =>
            {
                r.ToTable("Rol");
            });
            //AspNetUserRoles
            builder.Entity<IdentityUserRole<string>>(u =>
            {
                u.ToTable("UserRol");
            });
        }
             
    }
}
