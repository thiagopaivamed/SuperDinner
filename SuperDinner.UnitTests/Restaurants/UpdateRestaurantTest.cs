using Bogus;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators;

namespace SuperDinner.UnitTests.Restaurants
{
    public sealed class UpdateRestaurantTest : BaseRestaurantTest, IDisposable
    {
        private readonly Faker<UpdateRestaurantRequest> _fakeUpdateRestaurantRequest;
        private readonly List<Restaurant> _restaurants;

        public UpdateRestaurantTest()
        {
            _fakeUpdateRestaurantRequest = new Faker<UpdateRestaurantRequest>()
                .RuleFor(r => r.RestaurantId, f => Guid.NewGuid())
                 .RuleFor(r => r.Name, f => f.Company.CompanyName())
                 .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                 .RuleFor(r => r.ContactPhone, f => f.Phone.PhoneNumber())
                 .RuleFor(r => r.Address, f => f.Address.StreetAddress())
                 .RuleFor(r => r.Country, f => f.Address.Country())
                 .RuleFor(r => r.Latitude, f => f.Address.Latitude())
                 .RuleFor(r => r.Longitude, f => f.Address.Longitude())
                 .RuleFor(r => r.Price, f => f.Random.Double(1, 100))
                 .RuleFor(r => r.ClientsLimit, f => f.Random.Int(1, 100))
                 .RuleFor(r => r.LastModifiedDate, f => DateTime.UtcNow);

            _fakeUpdateRestaurantRequest.ShouldNotBe(null);

            _restaurants = _fakeRestaurant.Generate(20);
            _restaurants.ShouldNotBe(null);
            _restaurants.Count.ShouldBeGreaterThanOrEqualTo(20);
        }

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_True()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = _fakeUpdateRestaurantRequest.Generate();
            UpdateRestaurantRequestValidator validator = new UpdateRestaurantRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            restaurantValidationResult.IsValid.ShouldBeTrue();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_False()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            UpdateRestaurantRequestValidator validator = new UpdateRestaurantRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            restaurantValidationResult.IsValid.ShouldBeFalse();
            #endregion
        }

        [Fact]
        public async Task Update_Valid_Dinner_Should_Return_Success()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = _fakeUpdateRestaurantRequest.Generate();
            updateRestaurantRequest.ShouldNotBe(null);

            Restaurant restaurant = _fakeRestaurant.Generate();
            restaurant.ShouldNotBe(null);

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, 200, null);
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBe(null);
            restaurantResponse.Messages.ShouldBe(null);

            _mockRestaurantHandler.Setup(r => r.UpdateRestaurantAsync(updateRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantUpdated = await _mockRestaurantHandler.Object.UpdateRestaurantAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantUpdated.ShouldNotBe(null);
            responseAfterRestaurantUpdated.IsSuccess.ShouldBeTrue();
            responseAfterRestaurantUpdated.Data.ShouldNotBe(null);
            responseAfterRestaurantUpdated.Messages.ShouldBe(null);
            #endregion
        }

        [Fact]
        public async Task Update_Invalid_Dinner_Should_Return_Failure()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            updateRestaurantRequest.ShouldNotBe(null);

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, 404, new List<string>() { "Invalid request" });
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBe(null);
            restaurantResponse.Messages.ShouldNotBe(null);

            _mockRestaurantHandler.Setup(r => r.UpdateRestaurantAsync(updateRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantAdded = await _mockRestaurantHandler.Object.UpdateRestaurantAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantAdded.ShouldNotBe(null);
            responseAfterRestaurantAdded.IsSuccess.ShouldBeFalse();
            responseAfterRestaurantAdded.Data.ShouldBe(null);
            responseAfterRestaurantAdded.Messages.ShouldNotBe(null);
            #endregion
        }

        public void Dispose() => _mockRestaurantHandler.VerifyAll();
    }
}
