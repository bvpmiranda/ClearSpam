using ClearSpam.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClearSpam.Entities
{
    public class Rule
    {
        private Account _account;

        public int Id { get; set; }
        public int AccountId { get; set; }
        [MaxLength(50)]
        public string Field { get; set; }
        [MaxLength(255)]
        public string Content { get; set; }

        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;

                if (_account != null)
                {
                    AccountId = _account.Id;
                }
            }
        }

        public override string ToString()
        {
            return $"{Field}: {Content}";
        }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rule>(entity =>
            {
                entity.ToTable("rule");

                entity.Property(x => x.Id)
                    .HasColumnName(nameof(Id).ToLower());

                entity.Property(x => x.AccountId)
                    .HasColumnName(nameof(AccountId).ToLower());

                entity.Property(x => x.Field)
                    .HasColumnName(nameof(Field).ToLower())
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(x => x.Content)
                    .HasColumnName(nameof(Content).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(true);

                entity.HasOne(x => x.Account)
                    .WithMany(x => x.Rules);
            });
        }
    }
}
