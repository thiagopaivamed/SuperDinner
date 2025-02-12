using SuperDinner.Application.Common.Api;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Application.Endpoints.Dinners
{
    public sealed class CreateDinnerEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
            => endpointRouteBuilder.MapPost("/", HandleAsync)
            .WithName("Dinners: Create")
            .WithSummary("Create a new Dinner")
            .WithDescription("Creates a new dinner in the system")
            .WithOrder(3)
            .Produces<Response<Dinner?>>()
            .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(IDinnerHandler dinnerHandler, CreateDinnerRequest request)
        {
            Response<Dinner> dinnerCreatedResponse = await dinnerHandler.AddDinnerAsync(request);

            return dinnerCreatedResponse.IsSuccess
                            ? Results.Created($"/dinners/{dinnerCreatedResponse.Data?.DinnerId}", dinnerCreatedResponse.Data)
                            : Results.BadRequest(dinnerCreatedResponse.Data);
        }
    }
}
