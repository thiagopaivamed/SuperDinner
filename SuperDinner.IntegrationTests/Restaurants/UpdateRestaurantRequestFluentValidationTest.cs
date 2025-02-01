using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Infrastructure.Data.Configuration;
using SuperDinner.Service.Validators;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class UpdateRestaurantRequestFluentValidationTest : IClassFixture<ValidationFixture<Restaurant>>
    {
        private readonly ValidationFixture<Restaurant> _validationFixture;
        private readonly UpdateRestaurantRequestValidator _updateRestaurantRequestValidator;
        private readonly EntityTypeBuilder<Restaurant> _restaurantEntityTypeBuilder;

        public UpdateRestaurantRequestFluentValidationTest(ValidationFixture<Restaurant> validationFixture)
        {
            _validationFixture = validationFixture;
            _updateRestaurantRequestValidator = new UpdateRestaurantRequestValidator();
            _restaurantEntityTypeBuilder = _validationFixture.GetEntityTypeBuilder<Restaurant, RestaurantConfiguration>();

            _validationFixture.ShouldNotBe(null);
            _updateRestaurantRequestValidator.ShouldNotBe(null);
            _restaurantEntityTypeBuilder.ShouldNotBe(null);
        }

        [Fact]
        public void Validator_MaxLength_Should_Have_Same_Count_As_MaxLength_In_Fluent_Api_Configuration()
        {
            #region Arrange
            string[] propertiesToValidate = _validationFixture.GetPropertyNames<string>();
            #endregion

            #region Act
            Dictionary<string, ILengthValidator> propertiesWithMaxLengthValidation = _validationFixture
                .GetFluentValidationProperties<UpdateRestaurantRequest, ILengthValidator>(propertiesToValidate, _updateRestaurantRequestValidator);

            Dictionary<string, IMutableProperty?> propertiesWithMaxLengthConfiguration = _validationFixture
                .GetFluentEfApiConfigurationProperties(propertiesToValidate, _restaurantEntityTypeBuilder);
            #endregion

            #region Assert
            propertiesToValidate.Length.ShouldBe(_validationFixture.stringsCount);

            propertiesWithMaxLengthValidation.ShouldNotBeNull();
            propertiesWithMaxLengthValidation.Count.ShouldBeGreaterThanOrEqualTo(_validationFixture.stringsCount);

            propertiesWithMaxLengthConfiguration.ShouldNotBeNull();
            propertiesWithMaxLengthConfiguration.Count.ShouldBeGreaterThanOrEqualTo(_validationFixture.stringsCount);

            foreach (KeyValuePair<string, ILengthValidator> propertyToValidate in propertiesWithMaxLengthValidation)
            {
                IMutableProperty? efProperty = propertiesWithMaxLengthConfiguration[propertyToValidate.Key];

                efProperty.ShouldNotBeNull($"Property {propertyToValidate.Key} should not be null");
                efProperty.GetMaxLength().ShouldBe(propertyToValidate.Value.Max, $"Property {propertyToValidate.Key} should have {efProperty.GetMaxLength()} max length");
            }
            #endregion
        }

        [Fact]
        public void Validator_NotEmpty_Should_Have_Same_Count_As_Required_In_Fluent_Api_Configuration()
        {
            #region Arrange
            string[] propertiesToIgnore =
            [
                "RestaurantId",
                "CreatedDate",
                "LastModifiedDate",
                "Dinners"
            ];

            int notEmptyPropertiesCount = _validationFixture.propertiesCount - propertiesToIgnore.Length;
            string[] propertiesToValidate = _validationFixture.GetPropertyNames(propertiesToIgnore);
            #endregion

            #region Act
            Dictionary<string, INotEmptyValidator> propertiesWithNotEmptyValidation = _validationFixture
                .GetFluentValidationProperties<UpdateRestaurantRequest, INotEmptyValidator>(propertiesToValidate, _updateRestaurantRequestValidator);

            Dictionary<string, IMutableProperty?> propertiesWithRequiredConfiguration = _validationFixture
                .GetFluentEfApiConfigurationProperties(propertiesToValidate, _restaurantEntityTypeBuilder);
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
