using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Domain.Interfaces.Restaurants.Handlers
{
    public interface IRestaurantHandler
    {
        Task<Response<Restaurant>> AddRestaurantAsync(CreateRestaurantRequest request);

        Task<Response<Restaurant>> GetRestaurantByIdAsync(GetRestaurantByIdRequest request);

        Task<Response<Restaurant>> UpdateRestaurantAsync(UpdateRestaurantRequest request);

        Task<Response<Restaurant>> DeleteRestaurantAsync(DeleteRestaurantRequest request);

        Task<PagedResponse<List<Restaurant>>> GetAllRestaurantsAsync(GetAllRestaurantsRequest request);
    }
}
