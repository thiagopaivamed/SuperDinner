using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Infrastructure.Data.Configuration;
using SuperDinner.Service.Validators.Dinner;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class CreateDinnerRequestFluentValidationTest : IClassFixture<ValidationFixture<Dinner>>
    {
        private readonly ValidationFixture<Dinner> _dinnerValidationFixture;
        private readonly CreateDinnerRequestValidator _createDinnerRequestValidator;
        private readonly EntityTypeBuilder<Dinner> _dinnerEntityTypeBuilder;

        public CreateDinnerRequestFluentValidationTest(ValidationFixture<Dinner> dinnerValidationFixture)
        {
            _dinnerValidationFixture = dinnerValidationFixture;
            _createDinnerRequestValidator = new CreateDinnerRequestValidator();
            _dinnerEntityTypeBuilder = _dinnerValidationFixture.GetEntityTypeBuilder<Dinner, DinnerConfiguration>();

            _dinnerValidationFixture.ShouldNotBeNull();
            _createDinnerRequestValidator.ShouldNotBeNull();
            _dinnerEntityTypeBuilder.ShouldNotBeNull();
        }        

        [Fact]
        public void Validator_NotEmpty_Should_Have_Same_Count_As_Required_In_Fluent_Api_Configuration()
        {
            #region Arrange
            string[] propertiesToIgnore =
            [
                "DinnerId",
                "Restaurant",
                "DinnerStatus",
                "LastModifiedDate"
            ];

            int notEmptyPropertiesCount = _dinnerValidationFixture.propertiesCount - propertiesToIgnore.Length;
            string[] propertiesToValidate = _dinnerValidationFixture.GetPropertyNames(propertiesToIgnore);
            #endregion

            #region Act
            Dictionary<string, INotEmptyValidator> propertiesWithNotEmptyValidation = _dinnerValidationFixture
                .GetFluentValidationProperties<CreateDinnerRequest, INotEmptyValidator>(propertiesToValidate, _createDinnerRequestValidator);

            Dictionary<string, IMutableProperty?> propertiesWithRequiredConfiguration = _dinnerValidationFixture
                .GetFluentEfApiConfigurationProperties(propertiesToValidate, _dinnerEntityTypeBuilder);
            #endregion

            #region Assert
            propertiesToValidate.Length.ShouldBe(notEmptyPropertiesCount);

            propertiesWithNotEmptyValidation.ShouldNotBeNull();
            propertiesWithNotEmptyValidation.Count.ShouldBe(notEmptyPropertiesCount);

            propertiesWithRequiredConfiguration.ShouldNotBeNull();
            propertiesWithRequiredConfiguration.Count.ShouldBe(notEmptyPropertiesCount);

            foreach (KeyValuePair<string, INotEmptyValidator> propertyToValidate in propertiesWithNotEmptyValidation)
            {
                IMutableProperty? efProperty = propertiesWithRequiredConfiguration[propertyToValidate.Key];

                efProperty?.IsNullable.ShouldBeFalse($"Property {propertyToValidate.Key} should be required on EF configuration");
                propertyToValidate.Value.Name.ShouldBe("NotEmptyValidator", $"Validator for {propertyToValidate.Key} should be NotEmptyValidator");
            }
            #endregion
        }
    }
}
