using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Entities;
using ClearSpam.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClearSpam.Persistence
{
    public class ClearSpamContext : IdentityDbContext
    {
        private readonly IClearSpamConfigurations configurations;

        public ClearSpamContext(DbContextOptions<ClearSpamContext> options, IClearSpamConfigurations configurations) : base(options)
        {
            this.configurations = configurations;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configurations.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurations(typeof(Interfaces.IConfiguration));
        }
    }
}