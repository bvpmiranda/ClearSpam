using ClearSpam.Domain.Entities;
using ClearSpam.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearSpam.Persistence.Configurations
{
    class FieldConfigurations : IEntityTypeConfiguration<Field>, IConfiguration
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable("Field");

            builder.Property(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Domain.Configurations.FieldConfigurations.NameMaxLength)
                .IsUnicode(false);
        }
    }
}