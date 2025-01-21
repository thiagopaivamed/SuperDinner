using FluentValidation.Results;
using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Domain.Responses;
using SuperDinner.Service.Validators;

namespace SuperDinner.Service.Handlers
{
    public class RestaurantHandler(IGenericRepository<Restaurant> repository, IUnitOfWork unitOfWork) : IRestaurantHandler
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
            restaurant.LastModifiedDate = request.LastModifiedDate;

            await repository.InsertAsync(restaurant);
            await unitOfWork.CommitAsync();

            return new Response<Restaurant>(restaurant, 200, []);
        }
    }
}
