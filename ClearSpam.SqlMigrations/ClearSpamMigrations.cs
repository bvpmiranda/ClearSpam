using Microsoft.EntityFrameworkCore;

namespace ClearSpam.SqlMigrations
{
    public static class ClearSpamMigrations
    {
        public static void ApplyMigrations(ClearSpamContext context)
        {
            context.Database.Migrate();
        }
    }
}
