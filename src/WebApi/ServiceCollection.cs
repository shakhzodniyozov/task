using System.Text;
using Application.Common.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using WebApi.Common.Services;
using WebApi.Settings;

namespace WebApi;

public static class ServiceCollection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, 
                                                                    WebApplicationBuilder builder)
    {
        services.AddHttpContextAccessor();
        services.AddSwaggerGen(setup =>
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

        services.AddScoped<IAuthService, AuthService>();

        builder.Host.UseSerilog((context, logger) =>
        {
            logger.WriteTo.Console();
        });
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(0)
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
        services.AddAuthorization();
        services.AddMassTransit(configure =>
        {
            configure.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                configurator.Host(rabbitMqSettings!.Host);
                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("ProductUser",false));
            });
        });

        return services;
    }
}