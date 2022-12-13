using FinalTask.Domain.Models;
using FinalTask.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Emit;

namespace FinalTask.Infrastucture.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            var ADMIN_ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";
            var USER_ROLE_ID = "341123f0-asd2–42de-afbf-59kmkkmk72cf7";

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                NormalizedName = "ADMIN",
                Name = "Admin",
                ConcurrencyStamp = ADMIN_ROLE_ID
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = USER_ROLE_ID,
                NormalizedName = "USER",
                Name = "User",
                ConcurrencyStamp = USER_ROLE_ID
            });

            AppUser user = new()
            {
                Id = ADMIN_ID,
                Email = "asd@asd.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN"
            };

            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            user.PasswordHash = ph.HashPassword(user, "String123@");

            builder.Entity<AppUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = USER_ROLE_ID,
                UserId = ADMIN_ID
            });

        }
    }
}
