using ClearSpam.Domain.Entities;
using ClearSpam.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearSpam.Persistence.Configurations
{
    class RuleConfigurations : IEntityTypeConfiguration<Rule>, IConfiguration
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.ToTable("Rule");

            builder.Property(x => x.Id);

            builder.Property(x => x.Field)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.RuleConfigurations.FieldMaxLength)
                .IsUnicode(false);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.RuleConfigurations.ContentMaxLength)
                .IsUnicode(false);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Rules);
        }
    }
}