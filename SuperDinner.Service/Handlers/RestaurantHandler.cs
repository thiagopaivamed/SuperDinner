using FluentValidation.Results;
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
        public async Task<Response<Restaurant>> AddRestaurantAsync(CreateRestaurantRequest request)
        {
            CreateRestaurantRequestValidator validator = new CreateRestaurantRequestValidator();
            ValidationResult result = await validator.ValidateAsync(request);
            if (!result.IsValid)
                return new Response<Restaurant>(null, 400, result.Errors.Select(x => x.ErrorMessage).ToList());

            Restaurant restaurant = new Restaurant();
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

            return new Response<Restaurant>(restaurant, 200);
        }

        public async Task<Response<Restaurant>> GetRestaurantByIdAsync(GetRestaurantByIdRequest request)
        {
            Restaurant restaurant = await repository.GetByIdAsync(request.RestaurantId);

            return restaurant is null ?
                new Response<Restaurant>(null, 404, ["Restaurant not found."]) :
                new Response<Restaurant>(restaurant, 200);
        }

        public async Task<Response<Restaurant>> UpdateRestaurantAsync(UpdateRestaurantRequest request)
        {
            Restaurant restaurant = await repository.GetByIdAsync(request.RestaurantId);

            if(restaurant is null)
                return new Response<Restaurant>(null, 404, ["Restaurant not found."]);

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

            return new Response<Restaurant>(restaurant, 200);
        }
    }
}
