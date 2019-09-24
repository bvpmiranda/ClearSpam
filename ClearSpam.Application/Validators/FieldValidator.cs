using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using FluentValidation;

namespace ClearSpam.Application.Validators
{
    public class FieldValidator : AbstractValidator<FieldDto>
    {
        public static readonly string FieldNotUnique = $"'{nameof(FieldDto.Name)}' is not unique";

        public FieldValidator(IRepository repository)
        {
            RuleFor(x => x.Name).MaximumLength(FieldConfigurations.NameMaxLength).NotEmpty();

            RuleFor(x => x).Custom((x, context) => {
                if (repository.Any<Field>(y => y.Name == x.Name && y.Id != x.Id))
                    context.AddFailure(nameof(RuleDto.Field), FieldNotUnique);
            });
        }
    }
}
