using Microsoft.EntityFrameworkCore;

namespace RMA.WebServer.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<RMAWebServerDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for MyProjectDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
