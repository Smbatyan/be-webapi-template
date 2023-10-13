using Application;
using Infrastructure;
using Microsoft.FeatureManagement;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog(); // Move to common
builder.AddCors(); // Move to common

builder.Services
    .AddEndpointsApiExplorer()
    .AddPandaSwaggerGen(builder.Configuration) // Move to common
    .AddCustomFluentValidation()
    .AddAppSettings(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApiVersioningFromHeader()
    .AddEndpointsApiExplorer()
    .AddFeatureManagement().Services
    .AddControllers();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UsePandaSwagger(builder.Configuration);

app.Run();