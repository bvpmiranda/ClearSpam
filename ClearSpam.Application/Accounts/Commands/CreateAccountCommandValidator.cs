using AutoMapper;
using ClearSpam.Application.Models;
using ClearSpam.Application.Validators;
using FluentValidation;

namespace ClearSpam.Application.Accounts.Commands
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(IMapper mapper)
        {
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