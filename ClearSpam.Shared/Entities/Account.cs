using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClearSpam.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        [MaxLength(255)]
        public string Server { get; set; }
        [Range(1, 65535)]
        public int Port { get; set; } = 993;
        public bool Ssl { get; set; } = true;
        [MaxLength(255)]
        public string Login { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        [NotMapped]
        public string OriginalPassword { get; set; }
        [DisplayName("Watched Mailbox")]
        public string WatchedMailbox { get; set; }
        public ICollection<Rule> Rules { get; set; } = new Collection<Rule>();

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(x => x.Id);

                entity.Property(x => x.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(true);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Server)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Port)
                    .IsRequired();

                entity.Property(x => x.Ssl)
                    .IsRequired();

                entity.Property(x => x.Login)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.WatchedMailbox)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
