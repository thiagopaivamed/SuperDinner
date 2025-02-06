using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Application.Common.Api;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class UpdateRestaurantEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
         => endpointRouteBuilder.MapPut("/", HandleAsync)
            .WithName("Restaurants: Update")
            .WithSummary("Update a restaurant")
            .WithDescription("Update a restaurant in the system")
            .WithOrder(4)
            .Produces<Response<Restaurant?>>()
            .Produces(StatusCodes.Status404NotFound);

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, UpdateRestaurantRequest request)
        {
            Response<Restaurant> restaurantUpdatedResponse = await restaurantHandler.UpdateRestaurantAsync(request);            

            return restaurantUpdatedResponse.IsSuccess 
                ? Results.Ok(restaurantUpdatedResponse.Data)
                : restaurantUpdatedResponse.ResponseStatusCode == StatusCodes.Status400BadRequest
                ? Results.BadRequest(restaurantUpdatedResponse.Data)
                : Results.NotFound(restaurantUpdatedResponse.Data);
        }
    }
}
