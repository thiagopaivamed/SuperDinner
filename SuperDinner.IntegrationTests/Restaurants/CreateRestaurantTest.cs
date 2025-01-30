using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class CreateRestaurantTest : IClassFixture<DependencyInjectionFixture>
    {
        private readonly DependencyInjectionFixture _dependencyInjectionFixture;

        private readonly Faker<CreateRestaurantRequest> _fakeCreateRestaurantRequest;

        private readonly IRestaurantHandler _restaurantHandler;

        private readonly IList<CreateRestaurantRequest> _restaurants;

        public CreateRestaurantTest()
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

            _restaurants = _fakeCreateRestaurantRequest.Generate(20);
            _restaurants.Count.ShouldBeGreaterThanOrEqualTo(20);

            _restaurantHandler = _dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();
            _restaurantHandler.ShouldNotBe(null);            
        }

        [Fact]
        public async Task Add_Valid_Restaurant_Should_Return_Success()
        {
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBe(null);

            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);

            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBe(null);
            responseRestaurantCreated.Messages.ShouldBe(null);            
        }

        [Fact]
        public async Task Add_Invalid_Restaurant_Should_Return_Bad_Request()
        {
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            createRestaurantRequest.ShouldNotBe(null);

            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);

            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeFalse();
            responseRestaurantCreated.Data.ShouldBe(null);
            responseRestaurantCreated.Messages.ShouldNotBe(null);
            responseRestaurantCreated.Messages.Count.ShouldBeGreaterThan(0);
        }
    }
}
