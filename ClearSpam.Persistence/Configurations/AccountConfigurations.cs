using ClearSpam.Domain.Entities;
using ClearSpam.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearSpam.Persistence.Configurations
{
    class AccountConfigurations : IEntityTypeConfiguration<Account>, IConfiguration
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            builder.Property(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.AccountConfigurations.UserIdMaxLength)
                .IsUnicode(true);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.AccountConfigurations.NameMaxLength)
                .IsUnicode(false);

            builder.Property(x => x.Server)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.AccountConfigurations.ServerMaxLength)
                .IsUnicode(false);

            builder.Property(x => x.Port)
                .IsRequired();

            builder.Property(x => x.Ssl)
                .IsRequired();

            builder.Property(x => x.Login)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.AccountConfigurations.LoginMaxLength)
                .IsUnicode(false);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.AccountConfigurations.PasswordMaxLength)
                .IsUnicode(false);

            builder.Property(x => x.WatchedMailbox)
                .HasMaxLength(Domain.Configurations.AccountConfigurations.WatchedMailboxMaxLength)
                .IsUnicode(false);
        }
    }
}