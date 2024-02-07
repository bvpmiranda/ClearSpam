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
                entity.ToTable("account");

                entity.Property(x => x.Id)
                    .HasColumnName(nameof(Id).ToLower());

                entity.Property(x => x.UserId)
                    .HasColumnName(nameof(UserId).ToLower())
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(true);

                entity.Property(x => x.Name)
                    .HasColumnName(nameof(Name).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Server)
                    .HasColumnName(nameof(Server).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Port)
                    .HasColumnName(nameof(Port).ToLower())
                    .IsRequired();

                entity.Property(x => x.Ssl)
                    .HasColumnName(nameof(Ssl).ToLower())
                    .IsRequired();

                entity.Property(x => x.Login)
                    .HasColumnName(nameof(Login).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.Password)
                    .HasColumnName(nameof(Password).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(x => x.WatchedMailbox)
                    .HasColumnName(nameof(WatchedMailbox).ToLower())
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
