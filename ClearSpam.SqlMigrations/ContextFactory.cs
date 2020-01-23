using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace ClearSpam.SqlMigrations
{
    public class ContextFactory : IDesignTimeDbContextFactory<ClearSpamContext>
    {
        public ClearSpamContext CreateDbContext(string connectionString)
        {
            var options = GetOptions(connectionString);
            var context = new ClearSpamContext(options);

            return context;
        }

        public ClearSpamContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }

        public static DbContextOptions<Persistence.ClearSpamContext> GetOptions(string connectionString)
        {
            var contextBuilder = new DbContextOptionsBuilder<Persistence.ClearSpamContext>()
                .UseSqlServer(connectionString);

            return contextBuilder.Options;
        }
    }
}
