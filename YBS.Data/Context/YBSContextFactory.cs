using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YBS.Data.Context
{
    public class YBSContextFactory : IDesignTimeDbContextFactory<YBSContext>
    {
        YBSContext IDesignTimeDbContextFactory<YBSContext>.CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + "/YBS";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile($"appsettings.{env}.json")
                .Build();
            var connectionStrings = configuration.GetConnectionString("YBSContext");
            var optionsBuilder = new DbContextOptionsBuilder<YBSContext>();
            optionsBuilder.UseSqlServer(connectionStrings);

            return new YBSContext(optionsBuilder.Options);
        }
    }
}