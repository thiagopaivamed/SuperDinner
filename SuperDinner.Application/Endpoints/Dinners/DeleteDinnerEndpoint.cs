using SuperDinner.Application.Common.Api;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Application.Endpoints.Dinners
{
    public sealed class DeleteDinnerEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
         => endpointRouteBuilder.MapDelete("/{dinnerId}", HandleAsync)
            .WithName("Dinners: Delete a dinner by its Id")
            .WithSummary("Deletes a dinner in the system")
            .WithDescription("Deletes a dinner in the system")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        private static async Task<IResult> HandleAsync(IDinnerHandler dinnerHandler, Guid dinnerId)
        {
            DeleteDinnerRequest request = new DeleteDinnerRequest(dinnerId);
            
            Response<Dinner> dinnerDeletedResponse = await dinnerHandler.DeleteDinnerAsync(request);

            return dinnerDeletedResponse.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(dinnerDeletedResponse.Data);
        }
    }
}
