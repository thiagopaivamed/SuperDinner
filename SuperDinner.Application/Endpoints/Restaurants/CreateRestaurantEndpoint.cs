using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Application.Common.Api;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class CreateRestaurantEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapPost("/", HandleAsync)
            .WithName("Restaurants: Create")
            .WithSummary("Create a new restaurant")
            .WithDescription("Creates a new restaurant in the system")
            .WithOrder(3)
            .Produces<Response<Restaurant?>>()
            .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, CreateRestaurantRequest request)
        {
            Response<Restaurant> restaurantCreatedResponse = await restaurantHandler.AddRestaurantAsync(request);
            return restaurantCreatedResponse.IsSuccess
                ? Results.Created($"/{restaurantCreatedResponse.Data?.RestaurantId}", restaurantCreatedResponse)
                : Results.BadRequest(restaurantCreatedResponse);
        }
    }
}
