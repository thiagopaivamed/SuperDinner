using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Infrastructure.Data.Configuration;
using SuperDinner.Service.Validators;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class RestaurantValidationTest : IClassFixture<ValidationFixture<Restaurant>>
    {
        private readonly ValidationFixture<Restaurant> _validationFixture;
        private readonly CreateRestaurantRequestValidator _createRestaurantRequestValidator;
        private readonly EntityTypeBuilder<Restaurant> _restaurantEntityTypeBuilder;

        public RestaurantValidationTest(ValidationFixture<Restaurant> validationFixture)
        {
            _validationFixture = validationFixture;
            _createRestaurantRequestValidator = new CreateRestaurantRequestValidator();
            _restaurantEntityTypeBuilder = _validationFixture.GetEntityTypeBuilder<Restaurant, RestaurantConfiguration>();

            _validationFixture.ShouldNotBe(null);
            _createRestaurantRequestValidator.ShouldNotBe(null);
            _restaurantEntityTypeBuilder.ShouldNotBe(null);
        }

        [Fact]
        public void Restaurant_Should_Have_Primary_Key()
        {
            #region Arrange
            string[] propertiesToValidate = _validationFixture.GetPropertyNames();
            Dictionary<string, IMutableProperty?> fluentApiConfigurationProperties = _validationFixture.GetFluentEfApiConfigurationProperties(propertiesToValidate, _restaurantEntityTypeBuilder);
            #endregion

            #region Act
            propertiesToValidate.Length.ShouldBe(_validationFixture.propertiesCount);

            int primaryKeyCount = fluentApiConfigurationProperties.Values.Count(property => property?.IsPrimaryKey() == true);
            #endregion

            #region Assert
            primaryKeyCount.ShouldBeGreaterThan(0, $"Restaurant should have at least one primary key");
            #endregion
        }

        [Fact]
        public void Validator_MaxLength_Should_Have_Same_Count_As_MaxLength_In_Fluent_Api_Configuration()
        {
            #region Arrange
            string[] propertiesToValidate = _validationFixture.GetPropertyNames<string>();
            #endregion

            #region Act
            Dictionary<string, ILengthValidator> propertiesWithMaxLengthValidation = _validationFixture
                .GetFluentValidationProperties<CreateRestaurantRequest, ILengthValidator>(propertiesToValidate, _createRestaurantRequestValidator);

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
                "LastModifiedDate",
                "Dinners"
            ];

            int notEmptyPropertiesCount = _validationFixture.propertiesCount - propertiesToIgnore.Length;
            string[] propertiesToValidate = _validationFixture.GetPropertyNames(propertiesToIgnore);
            #endregion

            #region Act
            Dictionary<string, INotEmptyValidator> propertiesWithNotEmptyValidation = _validationFixture
                .GetFluentValidationProperties<CreateRestaurantRequest, INotEmptyValidator>(propertiesToValidate, _createRestaurantRequestValidator);

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

        [Fact]
        public void Validator_Null_Should_Have_Same_Count_As_IsNotRequired_In_Fluent_Api_Configuration()
        {
            #region Arrange
            string[] propertiesToIgnore = _validationFixture.GetPropertyNames(["LastModifiedDate"]);

            int nullPropertiesCount = _validationFixture.propertiesCount - propertiesToIgnore.Length;

            string[] propertiesToValidate = _validationFixture.GetPropertyNames(propertiesToIgnore);
            #endregion

            #region Act
            Dictionary<string, INullValidator> propertiesWithNullConfiguration = _validationFixture
                .GetFluentValidationProperties<CreateRestaurantRequest, INullValidator>(propertiesToValidate, _createRestaurantRequestValidator);

            Dictionary<string, IMutableProperty?> propertiesWithRequiredConfiguration = _validationFixture
                .GetFluentEfApiConfigurationProperties(propertiesToValidate, _restaurantEntityTypeBuilder);
            #endregion

            #region Assert
            propertiesToValidate.Length.ShouldBe(nullPropertiesCount);

            propertiesWithNullConfiguration.ShouldNotBeNull();
            propertiesWithNullConfiguration.Count.ShouldBe(nullPropertiesCount);

            propertiesWithRequiredConfiguration.ShouldNotBeNull();
            propertiesWithRequiredConfiguration.Count.ShouldBe(nullPropertiesCount);

            foreach (KeyValuePair<string, INullValidator> propertyToValidate in propertiesWithNullConfiguration)
            {
                IMutableProperty? efProperty = propertiesWithRequiredConfiguration[propertyToValidate.Key];
                efProperty?.IsNullable.ShouldBeTrue($"Property {propertyToValidate.Key} should not be required on EF configuration");
                propertyToValidate.Value.Name.ShouldBe("NullValidator", $"Validator for {propertyToValidate.Key} should be NullValidator");
            }
            #endregion
        }
    }
}
