using Bogus;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators;

namespace SuperDinner.UnitTests
{
    public class RestaurantTest : IDisposable
    {
        private readonly Faker<Restaurant> fakeRestaurant;
        private readonly Faker<CreateRestaurantRequest> fakeCreateRestaurantRequest;
        private readonly List<Restaurant> restaurants;

        private readonly Mock<IRestaurantHandler> mockRestaurantHandler;
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

            fakeCreateRestaurantRequest = new Faker<CreateRestaurantRequest>()
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

            restaurants = fakeRestaurant.Generate(20);

            fakeRestaurant.ShouldNotBe(null);

            fakeCreateRestaurantRequest.ShouldNotBe(null);

            restaurants.ShouldNotBe(null);
            restaurants.Count.ShouldBe(20);

            mockRestaurantHandler = new Mock<IRestaurantHandler>();
        }

        [Fact]
        public async Task Use_Valid_Restaurant_Should_Return_True()
        {
            CreateRestaurantRequest createRestaurantRequest = fakeCreateRestaurantRequest.Generate();
            CreateRestaurantRequestValidator validator = new CreateRestaurantRequestValidator();

            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(createRestaurantRequest);

            restaurantValidationResult.IsValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Use_Invalid_Restaurant_Should_Return_False()
        {
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            CreateRestaurantRequestValidator validator = new CreateRestaurantRequestValidator();

            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(createRestaurantRequest);

            restaurantValidationResult.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Add_Valid_Dinner_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBe(null);

            Restaurant restaurant = fakeRestaurant.Generate();
            restaurant.ShouldNotBe(null);

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, 200, null);
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBe(null);
            restaurantResponse.Messages.ShouldBe(null);

            mockRestaurantHandler.Setup(r => r.AddRestaurantAsync(createRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantAdded = await mockRestaurantHandler.Object.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantAdded.ShouldNotBe(null);
            responseAfterRestaurantAdded.IsSuccess.ShouldBeTrue();
            responseAfterRestaurantAdded.Data.ShouldNotBe(null);
            responseAfterRestaurantAdded.Messages.ShouldBe(null);
            #endregion
        }

        [Fact]
        public async Task Add_Invalid_Dinner_Should_Return_Failure()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            createRestaurantRequest.ShouldNotBe(null);

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, 400, new List<string>() { "Invalid request" });
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBe(null);
            restaurantResponse.Messages.ShouldNotBe(null);

            mockRestaurantHandler.Setup(r => r.AddRestaurantAsync(createRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantAdded = await mockRestaurantHandler.Object.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantAdded.ShouldNotBe(null);
            responseAfterRestaurantAdded.IsSuccess.ShouldBeFalse();
            responseAfterRestaurantAdded.Data.ShouldBe(null);
            responseAfterRestaurantAdded.Messages.ShouldNotBe(null);
            #endregion
        }

        public void Dispose() => mockRestaurantHandler.VerifyAll();

    }
}
