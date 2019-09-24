using ClearSpam.Application.BaseMediator.Commands;
using FluentValidation;

namespace ClearSpam.Application.Accounts.Commands
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator()
        {
            RuleFor(x => x).Custom((x, context) => {
                var validator = new DeleteEntityCommandValidator();
                var result = validator.Validate(x);
                foreach (var error in result.Errors)
                    context.AddFailure(error.PropertyName, error.ErrorMessage);
            });
        }
    }
}