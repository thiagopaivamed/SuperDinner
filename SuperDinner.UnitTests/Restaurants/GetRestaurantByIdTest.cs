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
            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest();
            getRestaurantByIdRequest.RestaurantId = Guid.NewGuid();           

            Restaurant restaurant = _fakeRestaurant.Generate();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, 200, null);
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBe(null);
            restaurantResponse.Messages.ShouldBe(null);
            #endregion

            #region Act
            _mockRestaurantHandler.Setup(r => r.GetRestaurantByIdAsync(getRestaurantByIdRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> responseAfterGetById = await _mockRestaurantHandler.Object.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseAfterGetById.ShouldNotBe(null);
            responseAfterGetById.IsSuccess.ShouldBeTrue();
            responseAfterGetById.Data.ShouldNotBe(null);
            responseAfterGetById.Messages.ShouldBe(null);
            #endregion
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_NotFound()
        {
            #region Arrange
            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest();
            getRestaurantByIdRequest.RestaurantId = Guid.NewGuid();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, 404, new List<string>() { "Not found" });
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBe(null);
            restaurantResponse.Messages.ShouldNotBe(null);
            #endregion

            #region Act
            _mockRestaurantHandler.Setup(r => r.GetRestaurantByIdAsync(getRestaurantByIdRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> responseAfterGetById = await _mockRestaurantHandler.Object.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseAfterGetById.ShouldNotBe(null);
            responseAfterGetById.IsSuccess.ShouldBeFalse();
            responseAfterGetById.Data.ShouldBe(null);
            responseAfterGetById.Messages.ShouldNotBe(null);
            #endregion
        }
    }
}
