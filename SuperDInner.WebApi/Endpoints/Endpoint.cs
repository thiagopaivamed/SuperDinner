using SuperDInner.Application.Common.Api;
using SuperDInner.Application.Endpoints.Restaurants;

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
                .MapEndpoint<CreateRestaurantEndpoint>();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder endpointRouteBuilder) where TEndpoint : IEndpoint
        {
            TEndpoint.Map(endpointRouteBuilder);
            return endpointRouteBuilder;
        }
    }
}
