using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClearSpam.Entities
{
    public class Field
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Field>(entity =>
            {
                entity.ToTable("Field");

                entity.Property(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
