using Microsoft.EntityFrameworkCore;
using SuperDinner.Infrastructure.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<SuperDinnerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SuperDinnerConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

await app.RunAsync();