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
                entity.ToTable("field");

                entity.Property(x => x.Id)
                    .HasColumnName(nameof(Id).ToLower());

                entity.Property(x => x.Name)
                    .HasColumnName(nameof(Name).ToLower())
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
