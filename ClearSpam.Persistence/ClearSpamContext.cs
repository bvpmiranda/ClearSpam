using ClearSpam.Domain.Entities;
using ClearSpam.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClearSpam.Persistence
{
    public class ClearSpamContext : DbContext
    {
        public ClearSpamContext() {
        }

        public ClearSpamContext(DbContextOptions<ClearSpamContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurations(typeof(Interfaces.IConfiguration));
        }
    }
}