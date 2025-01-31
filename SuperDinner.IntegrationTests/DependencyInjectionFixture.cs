using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Domain.Interfaces.Restaurants;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.Infrastructure.Data.Context;
using SuperDinner.Infrastructure.Data.Repositories;
using SuperDinner.Service.Handlers;
using SuperDinner.Service.Validators;
using System.Reflection;

namespace SuperDinner.IntegrationTests
{
    public sealed class DependencyInjectionFixture : IDisposable
    {
        public readonly ServiceProvider serviceProvider;

        public DependencyInjectionFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SuperDinnerContext>(options => options.UseInMemoryDatabase("SuperDinnerTestDatabase"));

            serviceCollection.AddLogging();

            RegisterRepositories(serviceCollection);
            RegisterServices(serviceCollection);
            RegisterValidation(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRestaurantHandler, RestaurantHandler>();
        }

        private void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRestaurantRepository, RestaurantRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void RegisterValidation(IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateRestaurantRequest>, CreateRestaurantRequestValidator>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
        }

        public void Dispose() =>
            serviceProvider.Dispose();
    }
}