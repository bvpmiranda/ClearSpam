using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using FluentValidation;

namespace ClearSpam.Application.Validators
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Server).MaximumLength(AccountConfigurations.ServerMaxLength).NotEmpty();
            RuleFor(x => x.Port).GreaterThanOrEqualTo(1).LessThanOrEqualTo(65535);
            RuleFor(x => x.Login).MaximumLength(AccountConfigurations.LoginMaxLength).NotEmpty();
            RuleFor(x => x.Password).MaximumLength(AccountConfigurations.PasswordMaxLength).NotEmpty();
        }
    }
}
