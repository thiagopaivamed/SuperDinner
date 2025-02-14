using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Infrastructure.Data.Configuration;

namespace SuperDinner.IntegrationTests.Dinners
{
    public sealed class DinnerFluentApiConfigurationTest : IClassFixture<ValidationFixture<Dinner>>
    {
        private readonly ValidationFixture<Dinner> _dinnerValidationFixture;
        private readonly EntityTypeBuilder<Dinner> _dinnerEntityTypeBuilder;

        public DinnerFluentApiConfigurationTest(ValidationFixture<Dinner> dinnerValidationFixture)
        {
            _dinnerValidationFixture = dinnerValidationFixture;
            _dinnerEntityTypeBuilder = _dinnerValidationFixture.GetEntityTypeBuilder<Dinner, DinnerConfiguration>();
        }

        [Fact]
        public void Dinner_Should_Have_Primary_Key()
        {
            string[] propertiesToValidate = _dinnerValidationFixture.GetPropertyNames();
            Dictionary<string, IMutableProperty?> fluentApiConfigurationProperties = _dinnerValidationFixture.GetFluentEfApiConfigurationProperties(propertiesToValidate, _dinnerEntityTypeBuilder);

            propertiesToValidate.Length.ShouldBe(_dinnerValidationFixture.propertiesCount);

            int primaryKeyCount = fluentApiConfigurationProperties.Values.Count(property => property?.IsPrimaryKey() == true);

            primaryKeyCount.ShouldBeGreaterThan(0, "Dinner should have a primary key");
        }
    }
}