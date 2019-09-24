using Microsoft.EntityFrameworkCore;

namespace ClearSpam.SqlMigrations
{
    public class ClearSpamContext : Persistence.ClearSpamContext
    {
        public ClearSpamContext()
        {
        }

        public ClearSpamContext(DbContextOptions<Persistence.ClearSpamContext> options) : base(options)
        {
        }
    }
}
