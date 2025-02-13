using Bogus;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators.Restaurant;

namespace SuperDinner.UnitTests.Restaurants
{
    public sealed class CreateRestaurantTest : BaseRestaurantTest, IDisposable
    {
        private readonly Faker<CreateRestaurantRequest> _fakeCreateRestaurantRequest;

        public CreateRestaurantTest()
        {            
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
        }

        [Fact]
        public async Task Use_Valid_Restaurant_Should_Return_True()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            CreateRestaurantRequestValidator validator = new CreateRestaurantRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(createRestaurantRequest);
            #endregion

            #region Assert
            restaurantValidationResult.IsValid.ShouldBeTrue();
            #endregion
        }

        [Fact]
        public async Task Use_Invalid_Restaurant_Should_Return_False()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            CreateRestaurantRequestValidator validator = new CreateRestaurantRequestValidator();
            #endregion

            #region Act
            FluentValidation.Results.ValidationResult restaurantValidationResult = await validator.ValidateAsync(createRestaurantRequest);
            #endregion

            #region Assert
            restaurantValidationResult.IsValid.ShouldBeFalse();
            #endregion
        }

        [Fact]
        public async Task Add_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBeNull();

            Restaurant restaurant = _fakeRestaurant.Generate();
            restaurant.ShouldNotBeNull();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, StatusCodes.Status201Created);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBeNull();
            restaurantResponse.Messages.ShouldBeNull();

            _mockRestaurantHandler.Setup(r => r.AddRestaurantAsync(createRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantAdded = await _mockRestaurantHandler.Object.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantAdded.ShouldNotBeNull();
            responseAfterRestaurantAdded.IsSuccess.ShouldBeTrue();
            responseAfterRestaurantAdded.Data.ShouldNotBeNull();
            responseAfterRestaurantAdded.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Add_Invalid_Restaurant_Should_Return_Failure()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            createRestaurantRequest.ShouldNotBeNull();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, StatusCodes.Status400BadRequest, ["Invalid restaurant data"]);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBeNull();
            restaurantResponse.Messages.ShouldNotBeNull();

            _mockRestaurantHandler.Setup(r => r.AddRestaurantAsync(createRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> responseAfterRestaurantAdded = await _mockRestaurantHandler.Object.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseAfterRestaurantAdded.ShouldNotBeNull();
            responseAfterRestaurantAdded.IsSuccess.ShouldBeFalse();
            responseAfterRestaurantAdded.Data.ShouldBeNull();
            responseAfterRestaurantAdded.Messages.ShouldNotBeNull();
            #endregion
        }

        public void Dispose() => _mockRestaurantHandler.VerifyAll();
    }
}
