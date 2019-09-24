using ClearSpam.Application.BaseMediator.Commands;
using FluentValidation;

namespace ClearSpam.Application.Rules.Commands
{
    public class DeleteRuleCommandValidator : AbstractValidator<DeleteRuleCommand>
    {
        public DeleteRuleCommandValidator()
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