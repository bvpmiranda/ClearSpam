using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClearSpam.SqlMigrations
{
    public class ContextFactory : IDesignTimeDbContextFactory<ClearSpamContext>
    {
        public ClearSpamContext CreateDbContext()
        {
            return CreateDbContext(new[] { "" });
        }

        public ClearSpamContext CreateDbContext(string[] args)
        {
            var options = GetOptions();
            var context = new ClearSpamContext(options);

            return context;
        }

        public static DbContextOptions<Persistence.ClearSpamContext> GetOptions()
        {
            var connectionString = ClearSpamContext.GetConnectionString();
            var contextBuilder = new DbContextOptionsBuilder<Persistence.ClearSpamContext>()
                .UseSqlServer(connectionString);

            return contextBuilder.Options;
        }
    }
}
