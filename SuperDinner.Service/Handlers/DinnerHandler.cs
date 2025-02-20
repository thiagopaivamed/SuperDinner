using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Domain.Interfaces.Dinners;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Requests.Dinner;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators.Dinner;

namespace SuperDinner.Service.Handlers
{
    public sealed class DinnerHandler(IDinnerRepository repository, IUnitOfWork unitOfWork) : IDinnerHandler
    {
        private const string dinnerNotFoundMessage = "Dinner not found.";

        public async Task<Response<Dinner>> AddDinnerAsync(CreateDinnerRequest request)
        {
            CreateDinnerRequestValidator createDinnerRequestValidator = new CreateDinnerRequestValidator();
            ValidationResult createDinnerRequestValidationResult = createDinnerRequestValidator.Validate(request);
            
            if (!createDinnerRequestValidationResult.IsValid)            
                return new Response<Dinner>(null, StatusCodes.Status400BadRequest,
                    [.. createDinnerRequestValidationResult.Errors.Select(x => x.ErrorMessage)]);

            Dinner dinner = new Dinner();
            dinner.DinnerId = Guid.NewGuid();
            dinner.DinnerDate = request.DinnerDate;
            dinner.UserId = request.UserId;
            dinner.RestaurantId = request.RestaurantId;
            dinner.TransactionId = request.TransactionId;
            dinner.DinnerStatus = request.DinnerStatus;
            dinner.CreatedDate = request.CreatedDate;

            await repository.InsertAsync(dinner);
            await unitOfWork.CommitAsync();

            return new Response<Dinner>(dinner, StatusCodes.Status201Created);
        }

        public async Task<Response<Dinner>> DeleteDinnerAsync(DeleteDinnerRequest request)
        {
            Dinner dinner = await repository.GetByIdAsync(request.DinnerId);

            if(dinner is null)
                return new Response<Dinner>(null, StatusCodes.Status404NotFound, [dinnerNotFoundMessage]);

            repository.Delete(dinner);
            await unitOfWork.CommitAsync();

            return new Response<Dinner>(dinner, StatusCodes.Status204NoContent);
        }

        public async Task<PagedResponse<IReadOnlyList<Dinner>>> GetAllDinnersAsync(GetAllDinnersRequest request)
        => await repository.GetAllAsync(request.PageNumber, request.PageSize);

        public async Task<Response<Dinner>> GetDinnerByIdAsync(GetDinnerByIdRequest request)
        {
            Dinner dinner = await repository.GetByIdAsync(request.DinnerId);

            return dinner is null ?
                new Response<Dinner>(null, StatusCodes.Status404NotFound, [dinnerNotFoundMessage]) :
                new Response<Dinner>(dinner, StatusCodes.Status200OK);
        }

        public async Task<Response<Dinner>> UpdateDinnerAsync(UpdateDinnerRequest request)
        {
            UpdateDinnerRequestValidator updateDinnerRequestValidator = new UpdateDinnerRequestValidator();
            ValidationResult updateDinnerRequestValidationResult = updateDinnerRequestValidator.Validate(request);

            if(!updateDinnerRequestValidationResult.IsValid)
                return new Response<Dinner>(null, StatusCodes.Status400BadRequest,
                    [.. updateDinnerRequestValidationResult.Errors.Select(x => x.ErrorMessage)]);

            Dinner dinner = await repository.GetByIdAsync(request.DinnerId);

            if (dinner is null)
                return new Response<Dinner>(null, StatusCodes.Status404NotFound, [dinnerNotFoundMessage]);

            dinner.DinnerDate = request.DinnerDate;
            dinner.UserId = request.UserId;
            dinner.RestaurantId = request.RestaurantId;
            dinner.TransactionId = request.TransactionId;
            dinner.DinnerStatus = request.DinnerStatus;
            dinner.LastModifiedDate = request.LastModifiedDate;

            repository.Update(dinner);
            await unitOfWork.CommitAsync();

            return new Response<Dinner>(dinner, StatusCodes.Status200OK);
        }
    }
}
