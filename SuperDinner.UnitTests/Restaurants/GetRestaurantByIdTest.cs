using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Restaurants
{
    public sealed class GetRestaurantByIdTest : BaseRestaurantTest
    {
        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest(Guid.NewGuid());

            Restaurant restaurant = _fakeRestaurant.Generate();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, StatusCodes.Status200OK);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBeNull();
            restaurantResponse.Messages.ShouldBeNull();
            #endregion

            #region Act
            _mockRestaurantHandler.Setup(r => r.GetRestaurantByIdAsync(getRestaurantByIdRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> responseAfterGetById = await _mockRestaurantHandler.Object.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseAfterGetById.ShouldNotBeNull();
            responseAfterGetById.IsSuccess.ShouldBeTrue();
            responseAfterGetById.Data.ShouldNotBeNull();
            responseAfterGetById.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_NotFound()
        {
            #region Arrange
            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest(Guid.NewGuid());

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, StatusCodes.Status404NotFound, ["Restaurant not found"]);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBeNull();
            restaurantResponse.Messages.ShouldNotBeNull();
            #endregion

            #region Act
            _mockRestaurantHandler.Setup(r => r.GetRestaurantByIdAsync(getRestaurantByIdRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> responseAfterGetById = await _mockRestaurantHandler.Object.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseAfterGetById.ShouldNotBeNull();
            responseAfterGetById.IsSuccess.ShouldBeFalse();
            responseAfterGetById.Data.ShouldBeNull();
            responseAfterGetById.Messages.ShouldNotBeNull();
            #endregion
        }
    }
}
