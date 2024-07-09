using Application;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("api/users").WithOpenApi()
                                            .WithTags("Users")
                                            .RequireAuthorization();

        root.MapGet("/", async (IMediator mediator, int pageIndex, int pageSize) => 
        {
            return Results.Ok(await mediator.Send(new GetAllUsersQuery(pageIndex, pageSize)));
        });

        root.MapPost("/login", async (IMediator mediator, LoginUserQuery query) =>
        {
            var loginResult = await mediator.Send(query);
            return loginResult.IsSuccess ? Results.Ok(loginResult.Value) : Results.BadRequest(loginResult.Reasons);
        }).AllowAnonymous();

        root.MapPost("/register", async (IMediator mediator, RegisterUserCommand command) =>
        {
            var registerResult = await mediator.Send(command);

            return registerResult.IsSuccess ? Results.Created($"/{registerResult.Value}", new {id = registerResult.Value}) 
                                            : Results.BadRequest(registerResult.Reasons);
        }).AddEndpointFilter<ValidationFilter<RegisterUserCommand>>();

        root.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetUserByIdQuery(id));

            return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Reasons);
        });

        root.MapPut("/", async (IMediator mediator, UpdateUserCommand command) =>
        {
            var response = await mediator.Send(command);

            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Reasons);
        }).AddEndpointFilter<ValidationFilter<UpdateUserCommand>>();

        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new DeleteUserCommand(id));

            return response.IsSuccess ? Results.NoContent() : Results.BadRequest(response.Reasons);
        });
    }
}