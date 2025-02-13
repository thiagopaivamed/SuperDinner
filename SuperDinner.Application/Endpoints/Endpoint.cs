using SuperDinner.Application.Endpoints.Restaurants;
using SuperDinner.Application.Common.Api;
using SuperDinner.Application.Endpoints.Dinners;

namespace SuperDinner.Application.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            RouteGroupBuilder endpoints = app.MapGroup("");

            endpoints.MapGroup("v1")
                .WithTags("Health Check")
                .MapGet("/", () => Results.Ok("Endpoint is up and running!"));

            endpoints.MapGroup("v1/restaurants")
                .WithTags("Restaurants")
                .MapEndpoint<GetAllRestaurantsEndpoint>()
                .MapEndpoint<GetRestaurantByIdEndpoint>()
                .MapEndpoint<CreateRestaurantEndpoint>()
                .MapEndpoint<UpdateRestaurantEndpoint>()
                .MapEndpoint<DeleteRestaurantEndpoint>();

            endpoints.MapGroup("v1/dinners")
                .WithTags("Dinners")
                .MapEndpoint<GetAllDinnersEndpoint>()
                .MapEndpoint<GetDinnerByIdEndpoint>()
                .MapEndpoint<CreateDinnerEndpoint>()
                .MapEndpoint<UpdateDinnerEndpoint>()
                .MapEndpoint<DeleteDinnerEndpoint>();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder endpointRouteBuilder) where TEndpoint : IEndpoint
        {
            TEndpoint.Map(endpointRouteBuilder);
            return endpointRouteBuilder;
        }
    }
}
