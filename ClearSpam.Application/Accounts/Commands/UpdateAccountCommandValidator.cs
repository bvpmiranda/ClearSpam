using AutoMapper;
using ClearSpam.Application.Models;
using ClearSpam.Application.Validators;
using ClearSpam.Domain.Configurations;
using FluentValidation;

namespace ClearSpam.Application.Accounts.Commands
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator(IMapper mapper)
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.WatchedMailbox).MaximumLength(AccountConfigurations.WatchedMailboxMaxLength).NotEmpty();
            RuleFor(x => x.TrashMailbox).MaximumLength(AccountConfigurations.TrashMailboxMaxLength).NotEmpty();

            RuleFor(x => x).Custom((x, context) => {
                var dto = mapper.Map<AccountDto>(x);
                var validator = new AccountValidator();
                var result = validator.Validate(dto);
                foreach (var error in result.Errors)
                    context.AddFailure(error.PropertyName, error.ErrorMessage);
            });
        }
    }
}