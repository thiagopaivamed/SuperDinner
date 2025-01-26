using Microsoft.EntityFrameworkCore;
using SuperDinner.Domain.Handlers;
using SuperDinner.Infrastructure.Data.Context;
using SuperDinner.Infrastructure.Data.Repositories;
using SuperDinner.Service.Handlers;

namespace SuperDinner.Application.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddDataContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SuperDinnerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SuperDinnerConnection")));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IRestaurantHandler, RestaurantHandler>();
        }
    }
}
