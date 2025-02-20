using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Interfaces.Restaurants;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Infrastructure.Data.Repositories;
using SuperDinner.Service.Handlers;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SuperDinner.Infrastructure.Data.Context;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Interfaces.Dinners;

namespace SuperDinner.IntegrationTests
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRestaurantHandler, RestaurantHandler>();
            serviceCollection.AddScoped<IDinnerHandler, DinnerHandler>();
        }

        public static void RegisterRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRestaurantRepository, RestaurantRepository>();
            serviceCollection.AddScoped<IDinnerRepository, DinnerRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void RegisterValidation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            serviceCollection.AddFluentValidationAutoValidation();
        }

        public static void RegisterLogging(this IServiceCollection serviceCollection)
            => serviceCollection.AddLogging();

        public static void RegisterDbContextForTesting(this IServiceCollection serviceCollection)
            => serviceCollection.AddDbContext<SuperDinnerContext>(options =>
                    options.UseInMemoryDatabase("SuperDinnerTestDatabase"),
                ServiceLifetime.Scoped);
    }
}
