using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class CreateRestaurantTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IRestaurantHandler _restaurantHandler;

        public CreateRestaurantTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _restaurantHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantCreated.ShouldNotBeNull();
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBeNull();
            responseRestaurantCreated.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_Bad_Request()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            createRestaurantRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantCreated.ShouldNotBeNull();
            responseRestaurantCreated.IsSuccess.ShouldBeFalse();
            responseRestaurantCreated.Data.ShouldBeNull();
            responseRestaurantCreated.Messages.ShouldNotBeNull();
            responseRestaurantCreated.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}