using SuperDinner.Domain.Entities;
using SuperDinner.Domain.Interfaces.Restaurants;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public sealed class RestaurantRepository(SuperDinnerContext superDinnerContext) : GenericRepository<Restaurant>(superDinnerContext), IRestaurantRepository;
}
