using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Domain.Interfaces.Restaurants;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators;

namespace SuperDinner.Service.Handlers
{
    public sealed class RestaurantHandler(IRestaurantRepository repository, IUnitOfWork unitOfWork) : IRestaurantHandler
    {
        private const string restaurantNotFoundMessage = "Restaurant not found.";

        public async Task<Response<Restaurant>> AddRestaurantAsync(CreateRestaurantRequest request)
        {
            CreateRestaurantRequestValidator createRestaurantRequestValidator = new CreateRestaurantRequestValidator();
            ValidationResult createRestaurantRequestValidationResult = await createRestaurantRequestValidator.ValidateAsync(request);

            if (!createRestaurantRequestValidationResult.IsValid)
                return new Response<Restaurant>(null, StatusCodes.Status400BadRequest,
                    createRestaurantRequestValidationResult.Errors.Select(x => x.ErrorMessage).ToList());

            Restaurant restaurant = new Restaurant();
            restaurant.RestaurantId = Guid.NewGuid();
            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.ContactPhone = request.ContactPhone;
            restaurant.Address = request.Address;
            restaurant.Country = request.Country;
            restaurant.Latitude = request.Latitude;
            restaurant.Longitude = request.Longitude;
            restaurant.ClientsLimit = request.ClientsLimit;
            restaurant.CreatedDate = request.CreatedDate;

            await repository.InsertAsync(restaurant);
            await unitOfWork.CommitAsync();

            return new Response<Restaurant>(restaurant, StatusCodes.Status201Created);
        }

        public async Task<Response<Restaurant>> DeleteRestaurantAsync(DeleteRestaurantRequest request)
        {
            Restaurant restaurant = await repository.GetByIdAsync(request.RestaurantId);

            if (restaurant is null)
                return new Response<Restaurant>(null, StatusCodes.Status404NotFound, [restaurantNotFoundMessage]);

            repository.Delete(restaurant);
            await unitOfWork.CommitAsync();

            return new Response<Restaurant>(restaurant, StatusCodes.Status204NoContent);
        }

        public async Task<PagedResponse<List<Restaurant>>> GetAllRestaurantsAsync(GetAllRestaurantsRequest request)
            => await repository.GetAllAsync(request.PageNumber, request.PageSize);

        public async Task<Response<Restaurant>> GetRestaurantByIdAsync(GetRestaurantByIdRequest request)
        {
            Restaurant restaurant = await repository.GetByIdAsync(request.RestaurantId);

            return restaurant is null ?
                new Response<Restaurant>(null, StatusCodes.Status404NotFound, [restaurantNotFoundMessage]) :
                new Response<Restaurant>(restaurant, StatusCodes.Status200OK);
        }

        public async Task<Response<Restaurant>> UpdateRestaurantAsync(UpdateRestaurantRequest request)
        {
            UpdateRestaurantRequestValidator updateRestaurantRequestValidator = new UpdateRestaurantRequestValidator();
            ValidationResult updateRestaurantRequestValidationResult = await updateRestaurantRequestValidator.ValidateAsync(request);
            if (!updateRestaurantRequestValidationResult.IsValid)
                return new Response<Restaurant>(null, StatusCodes.Status400BadRequest,
                    updateRestaurantRequestValidationResult.Errors.Select(x => x.ErrorMessage).ToList());

            Restaurant restaurant = await repository.GetByIdAsync(request.RestaurantId);

            if (restaurant is null)
                return new Response<Restaurant>(null, StatusCodes.Status404NotFound, [restaurantNotFoundMessage]);

            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.ContactPhone = request.ContactPhone;
            restaurant.Address = request.Address;
            restaurant.Country = request.Country;
            restaurant.Latitude = request.Latitude;
            restaurant.Longitude = request.Longitude;
            restaurant.ClientsLimit = request.ClientsLimit;
            restaurant.LastModifiedDate = request.LastModifiedDate;

            repository.Update(restaurant);
            await unitOfWork.CommitAsync();

            return new Response<Restaurant>(restaurant, StatusCodes.Status200OK);
        }
    }
}
