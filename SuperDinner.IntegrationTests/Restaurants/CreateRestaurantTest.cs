using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class CreateRestaurantTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        public CreateRestaurantTest() { }

        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBe(null);
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBe(null);
            responseRestaurantCreated.Messages.ShouldBe(null);
            #endregion
        }

        [Fact]
        public async Task Given_Invalid_Restaurant_Should_Return_Bad_Request()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = new CreateRestaurantRequest();
            createRestaurantRequest.ShouldNotBe(null);
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            #endregion

            #region Assert
            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeFalse();
            responseRestaurantCreated.Data.ShouldBe(null);
            responseRestaurantCreated.Messages.ShouldNotBe(null);
            responseRestaurantCreated.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}