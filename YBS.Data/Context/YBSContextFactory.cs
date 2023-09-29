using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using YBS.Data.Context;

public class YBSDbContextFactory : IDesignTimeDbContextFactory<YBSContext>
{
    YBSContext IDesignTimeDbContextFactory<YBSContext>.CreateDbContext(string[] args)
    {
                IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionStrings = configuration.GetConnectionString("YBSContext");
        var optionsBuilder = new DbContextOptionsBuilder<YBSContext>();
        optionsBuilder.UseSqlServer(connectionStrings);

        return new YBSContext(optionsBuilder.Options);
    }
}