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
            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(Guid.NewGuid());

            Restaurant restaurant = _fakeRestaurant.Generate();

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(restaurant, StatusCodes.Status204NoContent);
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeTrue();
            restaurantResponse.Data.ShouldNotBe(null);
            restaurantResponse.Messages.ShouldBe(null);

            _mockRestaurantHandler.Setup(r => r.DeleteRestaurantAsync(deleteRestaurantRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> restaurantDeletedResponse = await _mockRestaurantHandler.Object.DeleteRestaurantAsync(deleteRestaurantRequest);
            restaurantDeletedResponse.ShouldNotBe(null);
            restaurantDeletedResponse.IsSuccess.ShouldBeTrue();
            restaurantDeletedResponse.Data.ShouldNotBe(null);
            restaurantDeletedResponse.Messages.ShouldBe(null);
        }

        [Fact]
        public async Task Given_Non_Existing_Guid_Should_Return_Not_Found()
        {
            DeleteRestaurantRequest deleteRestaurantRequest = new DeleteRestaurantRequest(Guid.NewGuid());

            Response<Restaurant> restaurantResponse = new Response<Restaurant>(null, StatusCodes.Status404NotFound, ["Restaurant not found"]);
            restaurantResponse.ShouldNotBe(null);
            restaurantResponse.IsSuccess.ShouldBeFalse();
            restaurantResponse.Data.ShouldBe(null);
            restaurantResponse.Messages.ShouldNotBeNull();
            restaurantResponse.Messages.Count.ShouldBeGreaterThan(0);

            _mockRestaurantHandler.Setup(r => r.DeleteRestaurantAsync(deleteRestaurantRequest)).ReturnsAsync(restaurantResponse);

            Response<Restaurant> restaurantDeletedResponse = await _mockRestaurantHandler.Object.DeleteRestaurantAsync(deleteRestaurantRequest);
            restaurantDeletedResponse.ShouldNotBe(null);
            restaurantDeletedResponse.IsSuccess.ShouldBeFalse();
            restaurantDeletedResponse.Data.ShouldBe(null);
            restaurantDeletedResponse.Messages.ShouldNotBeNull();
            restaurantDeletedResponse.Messages.Count.ShouldBeGreaterThan(0);
        }
    }
}
