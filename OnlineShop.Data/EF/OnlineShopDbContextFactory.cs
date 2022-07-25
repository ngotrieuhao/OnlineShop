using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OnlineShop.Data.EF
{
    public class OnlineShopDbContextFactory : IDesignTimeDbContextFactory<OnlineShopDbContext>
    {
        public OnlineShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("OnlineShopDb");

            var optionsBuilder = new DbContextOptionsBuilder<OnlineShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OnlineShopDbContext(optionsBuilder.Options);
        }
    }
}
