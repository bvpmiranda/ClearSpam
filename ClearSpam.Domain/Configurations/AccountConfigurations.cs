using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Infrastructure;

namespace ClearSpam.Domain.Configurations
{
    public class AccountConfigurations : EntityConfigurations
    {
        public const int UserIdMaxLength = 450;
        public const int NameMaxLength = 255;
        public const int ServerMaxLength = 255;
        public const int LoginMaxLength = 255;
        public const int PasswordMaxLength = 255;
        public const int WatchedMailboxMaxLength = 255;

        public AccountConfigurations() : base(typeof(Account))
        {
        }
    }
}