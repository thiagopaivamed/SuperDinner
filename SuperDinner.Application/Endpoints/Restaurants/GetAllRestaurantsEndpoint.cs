using Microsoft.AspNetCore.Mvc;
using SuperDinner.Domain;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Application.Common.Api;
using Microsoft.Extensions.Caching.Memory;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class GetAllRestaurantsEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapGet("/", HandleAsync)
            .WithName("Restaurants: Get All")
            .WithSummary("Get all restaurants")
            .WithDescription("Get all restaurants in the system")
            .WithOrder(1)
            .Produces<PagedResponse<IReadOnlyList<Restaurant>>>();

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler,
            IMemoryCache memoryCache,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            string cacheKey = $"restaurants-{pageNumber}-{pageSize}";

            if (!memoryCache.TryGetValue(cacheKey, out PagedResponse<IReadOnlyList<Restaurant>>? pagedRestaurantsResponse))
            {
                GetAllRestaurantsRequest getAllRestaurantsRequest = new GetAllRestaurantsRequest();
                getAllRestaurantsRequest.PageNumber = pageNumber;
                getAllRestaurantsRequest.PageSize = pageSize;

                pagedRestaurantsResponse = await restaurantHandler.GetAllRestaurantsAsync(getAllRestaurantsRequest);

                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                memoryCache.Set(cacheKey, pagedRestaurantsResponse, memoryCacheEntryOptions);
            }

            return pagedRestaurantsResponse!.IsSuccess
                ? Results.Ok(pagedRestaurantsResponse)
                : Results.BadRequest(pagedRestaurantsResponse);
        }
    }
}
