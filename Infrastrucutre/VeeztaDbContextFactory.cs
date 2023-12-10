using Infrastrucutre;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class VeeztaDbContextFactory : IDesignTimeDbContextFactory<VeeztaDbContext>
{
    public VeeztaDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the connection string
        var connectionString = configuration.GetConnectionString("MyConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Could not find a connection string named 'MyConnectionString'.");
        }

        // Build DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<VeeztaDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new VeeztaDbContext(optionsBuilder.Options);
    }
}