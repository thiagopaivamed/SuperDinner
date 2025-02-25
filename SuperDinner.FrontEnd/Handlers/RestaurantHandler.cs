using Microsoft.AspNetCore.Http;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using System.Net.Http.Json;

namespace SuperDinner.FrontEnd.Handlers
{
    public sealed class RestaurantHandler(IHttpClientFactory httpClientFactory) : IRestaurantHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<Response<Restaurant>> AddRestaurantAsync(CreateRestaurantRequest request)
        {
            HttpResponseMessage restaurantCreated = await _httpClient.PostAsJsonAsync("/v1/restaurants", request);

            return await restaurantCreated.Content.ReadFromJsonAsync<Response<Restaurant>>() ??
                new Response<Restaurant>(null!, StatusCodes.Status400BadRequest, ["Unable to create restaurant."]);
        }

        public async Task<Response<Restaurant>> DeleteRestaurantAsync(DeleteRestaurantRequest request)
        {
            HttpResponseMessage restaurantDeleted = await _httpClient.DeleteAsync($"/v1/Restaurants/{request.RestaurantId}");

            return await restaurantDeleted.Content.ReadFromJsonAsync<Response<Restaurant>>() ??
                new Response<Restaurant>(null!, StatusCodes.Status400BadRequest, ["Unable to delete restaurant."]);
        }

        public async Task<PagedResponse<IReadOnlyList<Restaurant>>> GetAllRestaurantsAsync(GetAllRestaurantsRequest request)
            => await _httpClient.GetFromJsonAsync<PagedResponse<IReadOnlyList<Restaurant>>>("/v1/Restaurants") ??
            new PagedResponse<IReadOnlyList<Restaurant>>(null!, StatusCodes.Status400BadRequest, ["Unable to get all restaurants."]);


        public async Task<Response<Restaurant>> GetRestaurantByIdAsync(GetRestaurantByIdRequest request)
            => await _httpClient.GetFromJsonAsync<Response<Restaurant>>($"/v1/Restaurants/{request.RestaurantId}") ??
            new Response<Restaurant>(null!, StatusCodes.Status400BadRequest, ["Unable to get restaurant."]);

        public async Task<Response<Restaurant>> UpdateRestaurantAsync(UpdateRestaurantRequest request)
        {
            HttpResponseMessage restaurantUpdated = await _httpClient.PutAsJsonAsync($"/v1/Restaurants/{request.RestaurantId}", request);

            return await restaurantUpdated.Content.ReadFromJsonAsync<Response<Restaurant>>() ??
                new Response<Restaurant>(null!, StatusCodes.Status400BadRequest, ["Unable to update restaurant."]);
        }
    }
}
