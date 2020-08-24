using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CulinaryBlogCore.DataAccess
{
    public class CulinaryDbContextFactory : IDesignTimeDbContextFactory<CulinaryBlogDbContext>
    {
        public CulinaryBlogDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new DbContextOptionsBuilder<CulinaryBlogDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new CulinaryBlogDbContext(builder.Options);
        }
    }
}
