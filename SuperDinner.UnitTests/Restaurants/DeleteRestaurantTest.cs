using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Restaurants
{
    public sealed class DeleteRestaurantTest : BaseRestaurantTest
    {
        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(Guid.NewGuid());

            Restaurant restaurant = _fakeRestaurant.Generate();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, StatusCodes.Status204NoContent);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBeNull();
            restaurantResponse.Messages.ShouldBeNull();

            _mockRestaurantHandler.Setup(r => r.DeleteRestaurantAsync(deleteRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion


            #region Act
            Response<Restaurant> restaurantDeletedResponse = await _mockRestaurantHandler.Object.DeleteRestaurantAsync(deleteRestaurantRequest);
            #endregion

            #region Assert
            restaurantDeletedResponse.ShouldNotBeNull();
            restaurantDeletedResponse.IsSuccess.ShouldBeTrue();
            restaurantDeletedResponse.Data.ShouldNotBeNull();
            restaurantDeletedResponse.Messages.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_Not_Found()
        {
            #region Arrange
            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(Guid.NewGuid());

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, StatusCodes.Status404NotFound, ["Restaurant not found"]);
            restaurantResponse.ShouldNotBeNull();
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBeNull();
            restaurantResponse.Messages.ShouldNotBeNull();
            restaurantResponse.Messages.Count.ShouldBeGreaterThan(0);

            _mockRestaurantHandler.Setup(r => r.DeleteRestaurantAsync(deleteRestaurantRequest)).ReturnsAsync(restaurantResponse);
            #endregion

            #region Act
            Response<Restaurant> restaurantDeletedResponse = await _mockRestaurantHandler.Object.DeleteRestaurantAsync(deleteRestaurantRequest);
            #endregion

            #region Assert
            restaurantDeletedResponse.ShouldNotBeNull();
            restaurantDeletedResponse.IsSuccess.ShouldBeFalse();
            restaurantDeletedResponse.Data.ShouldBeNull();
            restaurantDeletedResponse.Messages.ShouldNotBeNull();
            restaurantDeletedResponse.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}
