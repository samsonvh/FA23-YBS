using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YBS.Data.Configs;
using YBS.Data.Models;

namespace YBS.Data.Context
{
public class YBSContext : DbContext
    {
        public YBSContext(DbContextOptions<YBSContext> options):base(options)
        {
        
        }
       public DbSet<Account> Accounts { get; set; }
       public DbSet<Member> Members { get; set; }
       public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YBSContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
