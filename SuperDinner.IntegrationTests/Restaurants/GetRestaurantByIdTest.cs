using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class GetRestaurantByIdTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {        
        public GetRestaurantByIdTest() { }

        [Fact]
        public async Task Given_Existing_Guid_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBe(null);
            #endregion

            #region Act
            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBe(null);
            responseRestaurantCreated.Messages.ShouldBe(null);

            GetRestaurantByIdRequest getRestaurantByIdRequest = new GetRestaurantByIdRequest();
            getRestaurantByIdRequest.RestaurantId = responseRestaurantCreated.Data.RestaurantId;

            Response<Restaurant> responseRestaurantRetrieved = await _restaurantHandler.GetRestaurantByIdAsync(getRestaurantByIdRequest);
            #endregion

            #region Assert
            responseRestaurantRetrieved.ShouldNotBe(null);
            responseRestaurantRetrieved.IsSuccess.ShouldBeTrue();
            responseRestaurantRetrieved.Data.ShouldNotBe(null);
            responseRestaurantRetrieved.Data.RestaurantId.ShouldBeEquivalentTo(getRestaurantByIdRequest.RestaurantId);
            responseRestaurantRetrieved.Messages.ShouldBe(null);
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
            responseRestaurantRetrieved.ShouldNotBe(null);
            responseRestaurantRetrieved.IsSuccess.ShouldBeFalse();
            responseRestaurantRetrieved.Data.ShouldBe(null);
            responseRestaurantRetrieved.Messages.ShouldNotBe(null);
            responseRestaurantRetrieved.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}
