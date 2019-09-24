using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Infrastructure;

namespace ClearSpam.Domain.Configurations
{
    public class FieldConfigurations : EntityConfigurations
    {
        public const int NameMaxLength = 50;

        public FieldConfigurations() : base(typeof(Account))
        {
        }
    }
}