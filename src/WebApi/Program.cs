using Application;
using Application.Features.Users.Commands.UpdateUser.UpdateTestUser;
using Infrastructure;
using MediatR;
using Serilog;
using WebApi;
using WebApi.Common.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddPresentationServices(builder);

    var app = builder.Build();

    app.ConfigureApplication();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<IMediator>();
    await service.Send(new UpdateTestUserPasswordCommand());

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}