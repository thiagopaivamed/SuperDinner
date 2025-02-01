using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.IntegrationTests.Restaurants
{
    public sealed class UpdateRestaurantTest : BaseRestaurantTest, IClassFixture<DependencyInjectionFixture>
    {
        [Fact]
        public async Task Given_Valid_Restaurant_Should_Return_Success()
        {
            #region Arrange
            CreateRestaurantRequest createRestaurantRequest = _fakeCreateRestaurantRequest.Generate();
            createRestaurantRequest.ShouldNotBe(null);

            Response<Restaurant> responseRestaurantCreated = await _restaurantHandler.AddRestaurantAsync(createRestaurantRequest);
            responseRestaurantCreated.ShouldNotBe(null);
            responseRestaurantCreated.IsSuccess.ShouldBeTrue();
            responseRestaurantCreated.Data.ShouldNotBe(null);
            responseRestaurantCreated.Messages.ShouldBe(null);
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
            responseRestaurantUpdated.ShouldNotBe(null);
            responseRestaurantUpdated.IsSuccess.ShouldBeTrue();
            responseRestaurantUpdated.Data.ShouldNotBe(null);
            responseRestaurantUpdated.Data.Name.ShouldBe(updateRestaurantRequest.Name);
            responseRestaurantUpdated.Data.Description.ShouldBe(updateRestaurantRequest.Description);
            responseRestaurantUpdated.Data.Address.ShouldBe(updateRestaurantRequest.Address);
            responseRestaurantUpdated.Data.Country.ShouldBe(updateRestaurantRequest.Country);
            responseRestaurantUpdated.Messages.ShouldBe(null);
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
            responseRestaurantUpdated.ShouldNotBe(null);
            responseRestaurantUpdated.IsSuccess.ShouldBeFalse();
            responseRestaurantUpdated.Data.ShouldBe(null);
            responseRestaurantUpdated.Messages.ShouldNotBe(null);
            responseRestaurantUpdated.Messages.Count.ShouldBeGreaterThan(0);
            #endregion
        }
    }
}