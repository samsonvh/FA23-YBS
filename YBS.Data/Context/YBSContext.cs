using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YBS.Data.Configs;
using YBS.Data.Models;
using YBS.Util.DateTracking;

namespace YBS.Data.Context
{
public class YBSContext : DbContext
    {
        public YBSContext(DbContextOptions<YBSContext> options) : base(options)
        {
        
        }
       public DbSet<Account> Accounts { get; set; }
       public DbSet<Member> Members { get; set; }
       public DbSet<Company> Companies { get; set; }
       public DbSet<Role> Role { get; set; }
       public DbSet<Dock> Docks { get; set; }
        public override Task<int> SaveChangesAsync (CancellationToken cancellationToken = default) 
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreationDate = DateTime.Now;
                    }
                    else 
                    {
                        changedOrAddedItem.LastModifiedDate = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync (cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YBSContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
