using Microsoft.EntityFrameworkCore;
using SuperDinner.Application.Common.Api;
using SuperDinner.Infrastructure.Data.Context;
using SuperDInner.Application.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.AddDataContext();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.MapEndpoints();

await app.RunAsync();