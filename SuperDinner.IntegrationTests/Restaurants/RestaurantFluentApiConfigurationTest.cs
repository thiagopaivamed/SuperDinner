using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Infrastructure.Data.Configuration;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class RestaurantFluentApiConfigurationTest : IClassFixture<ValidationFixture<Restaurant>>
    {
        private readonly ValidationFixture<Restaurant> _validationFixture;
        private readonly EntityTypeBuilder<Restaurant> _restaurantEntityTypeBuilder;

        public RestaurantFluentApiConfigurationTest(ValidationFixture<Restaurant> validationFixture)
        {
            _validationFixture = validationFixture;
            _restaurantEntityTypeBuilder = _validationFixture.GetEntityTypeBuilder<Restaurant, RestaurantConfiguration>();

            _validationFixture.ShouldNotBe(null);
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
    }
}
