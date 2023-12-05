using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ClearSpam.SqlMigrations
{
    public class ContextFactory : IDesignTimeDbContextFactory<ClearSpamMigrationContext>
    {
        public ClearSpamMigrationContext CreateDbContext(string connectionString)
        {
            var options = GetOptions(connectionString);
            var context = new ClearSpamMigrationContext(options);

            return context;
        }

        public ClearSpamMigrationContext CreateDbContext(string[] args)
        {
            if (args.Length <= 1)
            {
                var connectionString = args.Length == 0
                    ? GetConnectionString()
                    : args[0];
                return CreateDbContext(connectionString);
            }

            throw new ArgumentException(nameof(args));
        }

        public static DbContextOptions<Persistence.ClearSpamContext> GetOptions(string connectionString)
        {
            var contextBuilder = new DbContextOptionsBuilder<Persistence.ClearSpamContext>().UseMySQL(connectionString);

            return contextBuilder.Options;
        }

        public static string GetConnectionString()
        {
            var basePath = Directory.GetCurrentDirectory();
            var machineName = System.Environment.MachineName;
            var appsettingsFileName = "appsettings";

            appsettingsFileName = File.Exists(Path.Combine(basePath, $"{appsettingsFileName}.{machineName}.json"))
                ? $"{appsettingsFileName}.{machineName}.json"
                : $"{appsettingsFileName}.json";

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(appsettingsFileName);
            var configuration = configurationBuilder.Build();
            return configuration.GetConnectionString("ClearSpam");
        }
    }
}
