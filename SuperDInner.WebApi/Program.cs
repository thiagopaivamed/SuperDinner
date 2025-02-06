using SuperDinner.Application.Common.Api;
using SuperDInner.Application.Endpoints;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        builder.AddDataContext();

        builder.AddServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.MapOpenApi();

        app.UseHttpsRedirection();

        app.MapEndpoints();

        await app.RunAsync();
    }
}