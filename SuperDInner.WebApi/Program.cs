using SuperDinner.Application.Common.Api;
using SuperDInner.Application.Endpoints;

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