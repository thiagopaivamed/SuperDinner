using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class UpdateRestaurantTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IRestaurantHandler _restaurantHandler;

        public UpdateRestaurantTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _restaurantHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBeNull();

            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            responseRestaurantCreated.ShouldNotBeNull();
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBeNull();
            responseRestaurantCreated.Messages.ShouldBeNull();
            #endregion

            #region Act
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            updateRestaurantRequest.RestaurantId = responseRestaurantCreated.Data.RestaurantId;
            updateRestaurantRequest.Name = "Name updated";
            updateRestaurantRequest.Description = "Description updated";
            updateRestaurantRequest.ContactPhone = responseRestaurantCreated.Data.ContactPhone;
            updateRestaurantRequest.Address = "Address updated";
            updateRestaurantRequest.Country = "Country updated";
            updateRestaurantRequest.Latitude = responseRestaurantCreated.Data.Latitude;
            updateRestaurantRequest.Longitude = responseRestaurantCreated.Data.Longitude;
            updateRestaurantRequest.Price = responseRestaurantCreated.Data.Price;
            updateRestaurantRequest.ClientsLimit = responseRestaurantCreated.Data.ClientsLimit;

            Response<Restaurant> responseRestaurantUpdated = await _restaurantHandler.UpdateRestaurantAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantUpdated.ShouldNotBeNull();
            responseRestaurantUpdated.IsSuccess.ShouldBeTrue();
            responseRestaurantUpdated.Data.ShouldNotBeNull();
            responseRestaurantUpdated.Data.Name.ShouldBe(updateRestaurantRequest.Name);
            responseRestaurantUpdated.Data.Description.ShouldBe(updateRestaurantRequest.Description);
            responseRestaurantUpdated.Data.Address.ShouldBe(updateRestaurantRequest.Address);
            responseRestaurantUpdated.Data.Country.ShouldBe(updateRestaurantRequest.Country);
            responseRestaurantUpdated.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_Failure()
        {
            #region Arrange
            UpdateRestaurantRequest updateRestaurantRequest = new UpdateRestaurantRequest();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantUpdated = await _restaurantHandler.UpdateRestaurantAsync(updateRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantUpdated.ShouldNotBeNull();
            responseRestaurantUpdated.IsSuccess.ShouldBeFalse();
            responseRestaurantUpdated.Data.ShouldBeNull();
            responseRestaurantUpdated.Messages.ShouldNotBeNull();
            responseRestaurantUpdated.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}