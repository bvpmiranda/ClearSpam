using Microsoft.EntityFrameworkCore;

namespace ClearSpam.SqlMigrations
{
    public static class ClearSpamMigrations
    {
        public static void ApplyMigrations(ClearSpamMigrationContext context)
        {
            context.Database.Migrate();
        }
    }
}
