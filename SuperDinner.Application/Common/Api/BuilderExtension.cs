using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Templates.Themes;
using SerilogTracing;
using SerilogTracing.Expressions;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Domain.Interfaces.Dinners;
using SuperDinner.Domain.Interfaces.Dinners.Handlers;
using SuperDinner.Domain.Interfaces.Restaurants;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Infrastructure.Data.Context;
using SuperDinner.Infrastructure.Data.Repositories;
using SuperDinner.Service.Handlers;

namespace SuperDinner.Application.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddDataContext(this WebApplicationBuilder builder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Testing")
                builder.Services.AddDbContext<SuperDinnerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SuperDinnerConnection")));
        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            builder.Services.AddTransient<IDinnerRepository, DinnerRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IRestaurantHandler, RestaurantHandler>();
            builder.Services.AddTransient<IDinnerHandler, DinnerHandler>();
        }

        public static void AddLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.WriteTo.Console(Formatters.CreateConsoleTextFormatter(TemplateTheme.Code));
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            });

            using IDisposable? listener = new ActivityListenerConfiguration()
                .Instrument.AspNetCoreRequests()
                .TraceToSharedLogger();
        }

        public static void AddMemoryCache(this WebApplicationBuilder builder)
            => builder.Services.AddMemoryCache();

    }
}
