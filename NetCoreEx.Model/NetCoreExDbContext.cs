using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetCoreEx.Model.Entities;
using NetCoreEx.Model.Interfaces;

namespace NetCoreEx.Model
{
    public class NetCoreExDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public NetCoreExDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser>? AppUsers { get; set; }
        public DbSet<AppRole>? AppRoles { get; set; }
        public DbSet<LogHistory>? LogHistorys { get; set; }
        public DbSet<FormDemo>? FormDemos { get; set; }
        public DbSet<FormDemoDetail>? FormDemoDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
        }

        /// <summary>
        /// Ghi đè hàm SaveChanges, tự động truyền DateCreated và DateModified
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IModelExample changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }

                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NetCoreExDbContext>
    {
        public NetCoreExDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<NetCoreExDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new NetCoreExDbContext(builder.Options);
        }
    }
}