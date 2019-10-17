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
        public static readonly string RuleAlreadyExists = "A rule for '{0}: {1}' already exists";

        public RuleValidator(IRepository repository)
        {
            RuleFor(x => x.Field).MaximumLength(RuleConfigurations.FieldMaxLength).NotEmpty();
            RuleFor(x => x.Content).MaximumLength(RuleConfigurations.ContentMaxLength).NotEmpty();

            RuleFor(x => x).Custom((x, context) => {
                if (!repository.Any<Field>(y => y.Name == x.Field))
                { 
                    context.AddFailure(nameof(RuleDto.Field), FieldInvalid);
                    return;
                }

                if (repository.Any<Rule>(y => y.Id != x.Id && 
                                              y.AccountId == x.AccountId &&
                                              y.Field == x.Field && 
                                              y.Content == x.Content))
                {
                    context.AddFailure(nameof(RuleDto.Content), string.Format(RuleAlreadyExists, x.Field, x.Content));
                    return;
                }
            });
        }
    }
}
