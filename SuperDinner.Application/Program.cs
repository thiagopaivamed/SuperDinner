using Scalar.AspNetCore;
using Serilog;
using SuperDinner.Application.Common.Api;
using SuperDinner.Application.Endpoints;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        builder.AddDataContext();

        builder.AddLogging();

        builder.AddServices();

        builder.AddMemoryCache();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options
                .WithTitle("SuperDinner API")
                .AddMetadata("Version", "1.0.0")
                .WithDarkMode(true)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.MapEndpoints();

        await app.RunAsync();
    }
}