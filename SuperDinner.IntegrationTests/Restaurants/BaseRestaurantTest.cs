using Bogus;
using Shouldly;
using SuperDinner.Domain.Requests.Restaurant;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public class BaseRestaurantTest
    {
        protected readonly Faker<CreateRestaurantRequest> _fakeCreateRestaurantRequest;

        public BaseRestaurantTest()
        {
            SetEnvironmentForTesting();

            _fakeCreateRestaurantRequest = new Faker<CreateRestaurantRequest>()
               .RuleFor(r => r.Name, f => f.Company.CompanyName())
               .RuleFor(r => r.Description, f => f.Lorem.Sentence())
               .RuleFor(r => r.ContactPhone, f => f.Phone.PhoneNumber())
               .RuleFor(r => r.Address, f => f.Address.StreetAddress())
               .RuleFor(r => r.Country, f => f.Address.Country())
               .RuleFor(r => r.Latitude, f => f.Address.Latitude())
               .RuleFor(r => r.Longitude, f => f.Address.Longitude())
               .RuleFor(r => r.Price, f => Math.Round(new Random().NextDouble() * 10, 2))
               .RuleFor(r => r.ClientsLimit, f => f.Random.Int(10, 100))
               .RuleFor(r => r.CreatedDate, f => DateTime.UtcNow);

            _fakeCreateRestaurantRequest.ShouldNotBeNull();

            string testEnvironmentValue = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
            testEnvironmentValue.ShouldNotBeNullOrEmpty();
            testEnvironmentValue.ShouldBe("Testing");
        }

        private void SetEnvironmentForTesting()
                => Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }
}