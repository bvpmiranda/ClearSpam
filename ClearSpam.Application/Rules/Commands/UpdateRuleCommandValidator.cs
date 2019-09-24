using AutoMapper;
using ClearSpam.Application.Models;
using ClearSpam.Application.Validators;
using ClearSpam.Domain.Interfaces;
using FluentValidation;

namespace ClearSpam.Application.Rules.Commands
{
    public class UpdateRuleCommandValidator : AbstractValidator<UpdateRuleCommand>
    {
        public UpdateRuleCommandValidator(IRepository repository, IMapper mapper)
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x).Custom((x, context) => {
                var dto = mapper.Map<RuleDto>(x);
                var validator = new RuleValidator(repository);
                var result = validator.Validate(dto);
                foreach (var error in result.Errors)
                    context.AddFailure(error.PropertyName, error.ErrorMessage);
            });
        }
    }
}