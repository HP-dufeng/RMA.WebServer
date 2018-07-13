using RMA.WebServer.Configuration;
using RMA.WebServer.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RMA.WebServer.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class RMAWebServerDbContextFactory : IDesignTimeDbContextFactory<RMAWebServerDbContext>
    {
        public RMAWebServerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RMAWebServerDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(RMAWebServerConsts.ConnectionStringName)
            );

            return new RMAWebServerDbContext(builder.Options);
        }
    }
}