using Moq;
using Shouldly;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.UnitTests.Restaurants
{
    public class GetAllRestaurantsTest : BaseRestaurantTest
    {
        [Fact]
        public async Task Get_All_Restaurants_Should_Return_Success()
        {
            GetAllRestaurantsRequest getAllRestaurantsRequest = new GetAllRestaurantsRequest();
            getAllRestaurantsRequest.PageNumber = 1;
            getAllRestaurantsRequest.PageSize = 10;

            IReadOnlyList<Restaurant> restaurants = _fakeRestaurant.Generate(10);

            PagedResponse<IReadOnlyList<Restaurant>> pagedRestaurants = new PagedResponse<IReadOnlyList<Restaurant>>(data: restaurants, 
                totalCount: restaurants.Count, 
                currentPage: 1, 
                pageSize: 10);

            _mockRestaurantHandler.Setup(r => r.GetAllRestaurantsAsync(getAllRestaurantsRequest)).ReturnsAsync(pagedRestaurants);

            PagedResponse<IReadOnlyList<Restaurant>> responseAfterGetAll = await _mockRestaurantHandler.Object.GetAllRestaurantsAsync(getAllRestaurantsRequest);

            responseAfterGetAll.ShouldNotBeNull();
            responseAfterGetAll.IsSuccess.ShouldBeTrue();
            responseAfterGetAll.Data.ShouldNotBeNull();
            responseAfterGetAll.Data.Count.ShouldBe(10);
            responseAfterGetAll.Messages.ShouldBeNull();
        }
    }
}
