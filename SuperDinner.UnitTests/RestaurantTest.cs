using Bogus;
using Shouldly;
using SuperDinner.Domain.Entities;

namespace SuperDinner.UnitTests
{
    public class RestaurantTest : IAsyncDisposable
    {
        private readonly Faker<Restaurant> fakeRestaurant;
        private readonly List<Restaurant> restaurants;
        public RestaurantTest()
        {
            fakeRestaurant = new Faker<Restaurant>()
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.ContactPhone, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.Address, f => f.Address.StreetAddress())
                .RuleFor(r => r.Country, f => f.Address.Country())
                .RuleFor(r => r.Latitude, f => f.Address.Latitude())
                .RuleFor(r => r.Longitude, f => f.Address.Longitude())
                .RuleFor(r => r.Price, f => f.Random.Double(1, 100))
                .RuleFor(r => r.ClientsLimit, f => f.Random.Int(1, 100))
                .RuleFor(r => r.CreatedDate, f => f.Date.Past(1))
                .RuleFor(r => r.Dinners, f => new List<Dinner>());
            
            restaurants = fakeRestaurant.Generate(20);

            fakeRestaurant.ShouldNotBe(null);
            restaurants.ShouldNotBe(null);
            restaurants.Count.ShouldBe(20);

        }
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
