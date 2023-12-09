using ClearSpam.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClearSpam
{
    public class ClearSpamContext : IdentityDbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }

        public ClearSpamContext(DbContextOptions<ClearSpamContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Account.OnModelCreating(modelBuilder);
            Field.OnModelCreating(modelBuilder);
            Rule.OnModelCreating(modelBuilder);
        }
    }
}