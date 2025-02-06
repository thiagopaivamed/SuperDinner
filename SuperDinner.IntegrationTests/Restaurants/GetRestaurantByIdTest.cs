using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class GetRestaurantByIdTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        private readonly IRestaurantHandler _restaurantHandler;

        public GetRestaurantByIdTest(DependencyInjectionFixture dependencyInjectionFixture)
            => _restaurantHandler = dependencyInjectionFixture.serviceProvider.GetRequiredService<IRestaurantHandler>();

        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBeNull();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            responseRestaurantCreated.ShouldNotBeNull();
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBeNull();
            responseRestaurantCreated.Messages.ShouldBeNull();

            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest();
            getRestaurantByIdRequest.RestaurantId = responseRestaurantCreated.Data.RestaurantId;

            Response<Restaurant> responseRestaurantRetrieved = await _restaurantHandler.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseRestaurantRetrieved.ShouldNotBeNull();
            responseRestaurantRetrieved.IsSuccess.ShouldBeTrue();
            responseRestaurantRetrieved.Data.ShouldNotBeNull();
            responseRestaurantRetrieved.Data.RestaurantId.ShouldBeEquivalentTo(getRestaurantByIdRequest.RestaurantId);
            responseRestaurantRetrieved.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_NotFound()
        {
            #region Arrange
            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest();
            getRestaurantByIdRequest.RestaurantId = Guid.NewGuid();
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantRetrieved = await _restaurantHandler.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseRestaurantRetrieved.ShouldNotBeNull();
            responseRestaurantRetrieved.IsSuccess.ShouldBeFalse();
            responseRestaurantRetrieved.Data.ShouldBeNull();
            responseRestaurantRetrieved.Messages.ShouldNotBeNull();
            responseRestaurantRetrieved.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}
