using Bogus;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;

namespace SuperDinner.UnitTests.Restaurants
{
    public class BaseRestaurantTest
    {
        protected readonly Faker<Restaurant> _fakeRestaurant;

        protected readonly Mock<IRestaurantHandler> _mockRestaurantHandler;

        public BaseRestaurantTest()
        {
            _fakeRestaurant = new Faker<Restaurant>()
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.ContactPhone, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.Address, f => f.Address.StreetAddress())
                .RuleFor(r => r.Country, f => f.Address.Country())
                .RuleFor(r => r.Latitude, f => f.Address.Latitude())
                .RuleFor(r => r.Longitude, f => f.Address.Longitude())
                .RuleFor(r => r.Price, f => f.Random.Double(10, 100))
                .RuleFor(r => r.ClientsLimit, f => f.Random.Int(1, 100))
                .RuleFor(r => r.CreatedDate, f => f.Date.Past(1))
                .RuleFor(r => r.Dinners, f => new List<Dinner>());

            _fakeRestaurant.ShouldNotBeNull();

            _mockRestaurantHandler = new Mock<IRestaurantHandler>();
            _mockRestaurantHandler.ShouldNotBeNull();
        }        
    }
}
