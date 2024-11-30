using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Web.Models.General_Entities;

namespace web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        public DbSet<MenuConfig> MenuConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b => b.ToTable("Users"));

            builder.Entity<IdentityRole>(b => b.ToTable("Roles"));

            builder.Entity<IdentityUserRole<string>>(b => b.ToTable("UserRoles"));

            builder.Entity<IdentityUserClaim<string>>(b => b.ToTable("UserClaims"));

            builder.Entity<IdentityUserLogin<string>>(b => b.ToTable("UserLogins"));

            builder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("RoleClaims"));

            builder.Entity<IdentityUserToken<string>>(b => b.ToTable("UserTokens"));


        }

    }
}
