using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Domain.Interfaces.Restaurants.Handlers
{
    public interface IRestaurantHandler
    {
        Task<Response<Restaurant>> AddRestaurantAsync(CreateRestaurantRequest request);

        Task<Response<Restaurant>> GetRestaurantByIdAsync(GetRestaurantByIdRequest request);
    }
}
