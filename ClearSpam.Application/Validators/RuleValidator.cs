using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using FluentValidation;

namespace ClearSpam.Application.Validators
{
    public class RuleValidator : AbstractValidator<RuleDto>
    {
        public static readonly string FieldInvalid = $"'{nameof(RuleDto.Field)}' is invalid";

        public RuleValidator(IRepository repository)
        {
            RuleFor(x => x.Field).MaximumLength(RuleConfigurations.FieldMaxLength).NotEmpty();
            RuleFor(x => x.Content).MaximumLength(RuleConfigurations.ContentMaxLength).NotEmpty();

            RuleFor(x => x).Custom((x, context) => {
                if (!repository.Any<Field>(y => y.Name == x.Field))
                    context.AddFailure(nameof(RuleDto.Field), FieldInvalid);
            });
        }
    }
}
