using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;

namespace SuperDinner.Domain.Interfaces.Dinners.Handlers
{
    public interface IDinnerHandler 
    {
        Task<Response<Dinner>> AddDinnerAsync(CreateDinnerRequest request);
        Task<Response<Dinner>> GetDinnerByIdAsync(GetDinnerByIdRequest request);
        Task<Response<Dinner>> UpdateDinnerAsync(UpdateDinnerRequest request);
        Task<Response<Dinner>> DeleteDinnerAsync(DeleteDinnerRequest request);
        Task<PagedResponse<IReadOnlyList<Dinner>>> GetAllDinnersAsync(GetAllDinnersRequest request);
    }
}
