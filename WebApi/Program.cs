using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Serilog;
using WebApi.Extensions;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(setup =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    setup.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    setup.AddSecurityRequirement(securityRequirement);
    setup.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
});

builder.Host.UseSerilog((context, logger) =>
{
    logger.WriteTo.Console();
});

var app = builder.Build();
app.ConfigureApplication();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitializeDbContext();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();