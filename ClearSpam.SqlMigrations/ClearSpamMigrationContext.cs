using ClearSpam.SqlMigrations.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClearSpam.SqlMigrations
{
    public class ClearSpamMigrationContext : Persistence.ClearSpamContext
    {
        public ClearSpamMigrationContext(DbContextOptions<Persistence.ClearSpamContext> options) : base(options, new Configurations())
        {
        }
    }
}
