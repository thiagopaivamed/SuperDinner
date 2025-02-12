using Microsoft.Extensions.Caching.Memory;
using SuperDinner.Application.Common.Api;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Application.Endpoints.Dinners
{
    public class GetDinnerByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapGet("/{dinnerId}", HandleAsync)
                .WithName("Dinners: Get a dinner by its Id")
                .WithSummary("Get a dinner by its Id")
                .WithDescription("Get a dinner by its Id")
                .Produces<Response<Dinner>>()
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

        private static async Task<IResult> HandleAsync(IDinnerHandler dinnerHandler,
            IMemoryCache memoryCache,
            Guid dinnerId)
        {
            string cacheKey = $"dinner-{dinnerId}";

            if (!memoryCache.TryGetValue(cacheKey, out Response<Dinner>? dinnerFoundResponse))
            {
                GetDinnerByIdRequest request = new GetDinnerByIdRequest(dinnerId);                

                dinnerFoundResponse = await dinnerHandler.GetDinnerByIdAsync(request);

                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                memoryCache.Set(cacheKey, dinnerFoundResponse, memoryCacheEntryOptions);
            }

            return dinnerFoundResponse!.IsSuccess
                ? Results.Ok(dinnerFoundResponse.Data)
                : Results.NotFound(dinnerFoundResponse.Data);
        }
    }
}
