using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDInner.Application.Common.Api;

namespace SuperDinner.Application.Endpoints.Restaurants
{
    public sealed class DeleteRestaurantEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
         => endpointRouteBuilder.MapDelete("/{restaurantId}", HandleAsync)
            .WithName("Restaurants: Delete a restaurant by its Id")
            .WithSummary("Delete a restaurant by its Id")
            .WithDescription("Delete a restaurant by its Id")
            .WithOrder(5)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        public static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, Guid restaurantId)
        {
            DeleteRestaurantRequest resquest = new();
            resquest.RestaurantId = restaurantId;

            Response<Restaurant> restaurantResponse = await restaurantHandler.DeleteRestaurantAsync(resquest);

            return restaurantResponse.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(restaurantResponse.Data);
        }
    }
}
