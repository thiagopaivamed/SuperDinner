using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SuperDinner.Application.Common.Api;
using SuperDinner.Domain;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Application.Endpoints.Dinners
{
    public sealed class GetAllDinnersEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapGet("/", HandleAsync)
            .WithName("Dinners: Get all dinners")
            .WithSummary("Get all dinners in the system")
            .WithDescription("Get all dinners in the system")
            .Produces<PagedResponse<IReadOnlyList<Dinner>>>()
            .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(IDinnerHandler dinnerHandler,
            IMemoryCache memoryCache,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            string cacheKey = $"dinners-{pageNumber}-{pageSize}";

            if (!memoryCache.TryGetValue(cacheKey, out PagedResponse<IReadOnlyList<Dinner>>? pagedDinnersResponse))
            {
                GetAllDinnersRequest getAllDinnersRequest = new GetAllDinnersRequest();
                getAllDinnersRequest.PageNumber = pageNumber;
                getAllDinnersRequest.PageSize = pageSize;

                pagedDinnersResponse = await dinnerHandler.GetAllDinnersAsync(getAllDinnersRequest);

                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                memoryCache.Set(cacheKey, pagedDinnersResponse, memoryCacheEntryOptions);
            }

            return pagedDinnersResponse!.IsSuccess
                ? Results.Ok(pagedDinnersResponse.Data)
                : Results.BadRequest(pagedDinnersResponse.Data);
        }
    }
}
