using FluentValidation.Validators;
using FluentValidation;

namespace SuperDinner.IntegrationTests
{
    public static class FluentValidationExtensions
    {
        public static IPropertyValidator[] GetValidatorsForMember<TEntity>(this IValidator<TEntity> validator, string memberName)
        {
            IValidatorDescriptor descriptor = validator.CreateDescriptor();

            return descriptor.GetValidatorsForMember(memberName)
                .Select(x => x.Validator)
                .ToArray();
        }
    }
}
