using FluentValidation;

namespace ClearSpam.Application.BaseMediator.Commands
{
    public class DeleteEntityCommandValidator : AbstractValidator<DeleteEntityCommand>
    {
        public DeleteEntityCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}