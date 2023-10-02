using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YBS.Data.Configs;
using YBS.Data.Models;

namespace YBS.Data.Context
{
public class YBSDbContext : DbContext
    {
        public YBSDbContext(DbContextOptions<YBSDbContext> options) : base(options)
        {
        
        }
       public DbSet<Account> Accounts { get; set; }
       public DbSet<Member> Members { get; set; }
       public DbSet<Company> Companies { get; set; }
       public DbSet<Route> Routes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YBSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
