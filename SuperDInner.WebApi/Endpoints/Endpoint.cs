using SuperDinner.Application.Endpoints.Restaurants;
using SuperDInner.Application.Common.Api;

namespace SuperDInner.Application.Endpoints
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
                .MapEndpoint<GetRestaurantByIdEndpoint>()
                .MapEndpoint<CreateRestaurantEndpoint>()
                .MapEndpoint<UpdateRestaurantEndpoint>()
                .MapEndpoint<DeleteRestaurantEndpoint>();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder endpointRouteBuilder) where TEndpoint : IEndpoint
        {
            TEndpoint.Map(endpointRouteBuilder);
            return endpointRouteBuilder;
        }
    }
}
