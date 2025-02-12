using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Application.Common.Api;
using Microsoft.Extensions.Caching.Memory;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class GetRestaurantByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapGet("/{restaurantId}", HandleAsync)
            .WithName("Restaurants: Get by Id")
            .WithSummary("Get a restaurant by its Id")
            .WithDescription("Get a restaurant by its Id")
            .WithOrder(2)
            .Produces<Response<Restaurant?>>()
            .Produces(StatusCodes.Status404NotFound);

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, IMemoryCache memoryCache, Guid restaurantId)
        {
            string cacheKey = $"restaurant-{restaurantId}";

            if (!memoryCache.TryGetValue(cacheKey, out Response<Restaurant>? restaurantFoundResponse))
            {
                GetRestaurantByIdRequest request = new GetRestaurantByIdRequest(restaurantId);

                restaurantFoundResponse = await restaurantHandler.GetRestaurantByIdAsync(request);

                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                memoryCache.Set(restaurantId, restaurantFoundResponse, memoryCacheEntryOptions);
            }

            return restaurantFoundResponse!.IsSuccess
                ? Results.Ok(restaurantFoundResponse.Data)
                : Results.NotFound(restaurantFoundResponse.Data);
        }
    }
}
