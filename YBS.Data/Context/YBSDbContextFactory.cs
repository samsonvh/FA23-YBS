using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YBS.Data.Context
{
    public class YBSDbContextFactory : IDesignTimeDbContextFactory<YBSDbContext>
    {
        YBSDbContext IDesignTimeDbContextFactory<YBSDbContext>.CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + "/YBS";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile($"appsettings.{env}.json")
                .Build();
            var connectionStrings = configuration.GetConnectionString("YBSContext");
            var optionsBuilder = new DbContextOptionsBuilder<YBSDbContext>();
            optionsBuilder.UseSqlServer(connectionStrings);

            return new YBSDbContext(optionsBuilder.Options);
        }
    }
}