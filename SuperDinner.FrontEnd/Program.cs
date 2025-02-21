using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using SuperDinner.Domain.Interfaces.Restaurants.Handlers;
using SuperDinner.Domain.Requests.Restaurant;
using SuperDinner.FrontEnd;
using SuperDinner.FrontEnd.Handlers;
using SuperDinner.Service.Validators.Restaurant;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IRestaurantHandler, RestaurantHandler>();

builder.Services.AddTransient<IValidator<CreateRestaurantRequest>, CreateRestaurantRequestValidator>();


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient(Configuration.HttpClientName, options =>
{
    options.BaseAddress = new Uri(Configuration.BackendUrl);
});

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;

    config.PopoverOptions.CheckForPopoverProvider = false;
});

await builder.Build().RunAsync();
