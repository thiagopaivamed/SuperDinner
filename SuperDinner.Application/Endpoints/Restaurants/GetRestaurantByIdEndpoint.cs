﻿using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Application.Common.Api;

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

        private static async Task<IResult> HandleAsync(IRestaurantHandler restaurantHandler, Guid restaurantId)
        {
            GetRestaurantByIdRequest request = new();
            request.RestaurantId = restaurantId;

            Response<Restaurant> restaurantFoundResponse = await restaurantHandler.GetRestaurantByIdAsync(request);

            return restaurantFoundResponse.IsSuccess
                ? Results.Ok(restaurantFoundResponse.Data)
                : Results.NotFound(restaurantFoundResponse.Data);
        }
    }
}
