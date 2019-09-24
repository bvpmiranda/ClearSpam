using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Infrastructure;

namespace ClearSpam.Domain.Configurations
{
    public class RuleConfigurations : EntityConfigurations
    {
        public const int FieldMaxLength = 50;
        public const int ContentMaxLength = 255;

        public RuleConfigurations() : base(typeof(Rule))
        {
        }
    }
}