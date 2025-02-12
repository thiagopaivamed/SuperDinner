using SuperDinner.Application.Common.Api;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Application.Endpoints.Dinners
{
    public sealed class UpdateDinnerEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapPut("/", HandleAsync)
            .WithName("Dinners: Update")
            .WithSummary("Update a Dinner")
            .WithDescription("Updates a dinner in the system")
            .WithOrder(4)
            .Produces<Response<Dinner?>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        private static async Task<IResult> HandleAsync(IDinnerHandler dinnerHandler, UpdateDinnerRequest request)
        {
            Response<Dinner> dinnerUpdatedResponse = await dinnerHandler.UpdateDinnerAsync(request);

            return dinnerUpdatedResponse.IsSuccess
                ? Results.Ok(dinnerUpdatedResponse.Data)
                : dinnerUpdatedResponse.ResponseStatusCode == StatusCodes.Status400BadRequest
                ? Results.BadRequest(dinnerUpdatedResponse.Data)
                : Results.NotFound(dinnerUpdatedResponse.Data);
        }
    }
}
