using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDInner.Application.Common.Api;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class CreateRestaurantEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapPost("/", HandleAsync)
            .WithName("Restaurants: Create")
            .WithSummary("Create a new restaurant")
            .WithDescription("Creates a new restaurant in the system")
            .WithOrder(1)
            .Produces<Response<Restaurant?>>();

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, CreateRestaurantRequest request)
        {
            Response<Restaurant> restaurantResponse = await restaurantHandler.AddRestaurantAsync(request);
            return restaurantResponse.IsSuccess
                ? Results.Created($"/{restaurantResponse.Data?.RestaurantId}", restaurantResponse.Data)
                : Results.BadRequest(restaurantResponse.Data);
        }
    }
}
