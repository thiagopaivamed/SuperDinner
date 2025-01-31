using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public class BaseRestaurantTest
    {
        protected readonly DependencyInjectionFixture _dependencyInjectionFixture;

        protected readonly Faker<CreateRestaurantRequest> _fakeCreateRestaurantRequest;

        protected readonly IRestaurantHandler _restaurantHandler;

        public BaseRestaurantTest()
        {
            _dependencyInjectionFixture = new DependencyInjectionFixture();
            _dependencyInjectionFixture.ShouldNotBe(null);
            
            _fakeCreateRestaurantRequest = new Faker<CreateRestaurantRequest>()
               .RuleFor(r => r.Name, f => f.Company.CompanyName())
               .RuleFor(r => r.Description, f => f.Lorem.Sentence())
               .RuleFor(r => r.ContactPhone, f => f.Phone.PhoneNumber())
               .RuleFor(r => r.Address, f => f.Address.StreetAddress())
               .RuleFor(r => r.Country, f => f.Address.Country())
               .RuleFor(r => r.Latitude, f => f.Address.Latitude())
               .RuleFor(r => r.Longitude, f => f.Address.Longitude())
               .RuleFor(r => r.Price, f => f.Random.Double(1, 100))
               .RuleFor(r => r.ClientsLimit, f => f.Random.Int(1, 100))
               .RuleFor(r => r.CreatedDate, f => DateTime.UtcNow);

            _fakeCreateRestaurantRequest.ShouldNotBe(null);

            _restaurantHandler = _dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();
            _restaurantHandler.ShouldNotBe(null);
        }
    }
}
