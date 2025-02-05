using Microsoft.AspNetCore.Mvc;
using SuperDinner.Domain;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDInner.Application.Common.Api;

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
            .Produces<PagedResponse<List<Restaurant>>>();

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            GetAllRestaurantsRequest getAllRestaurantsRequest = new GetAllRestaurantsRequest();
            getAllRestaurantsRequest.PageNumber = pageNumber;
            getAllRestaurantsRequest.PageSize = pageSize;

            PagedResponse<List<Restaurant>> response = await restaurantHandler.GetAllRestaurantsAsync(getAllRestaurantsRequest);

            return response.IsSuccess
                ? Results.Ok(response)
                : Results.NotFound(response);
        }
    }
}
