using System.Reflection;
using Application.Features.Products.Commands.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            c.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
    }
}
