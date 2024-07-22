using Application.Common.Responses;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser.UpdateUser;
using Application.Features.Users.Queries.GetAllUsersQuery;
using Application.Features.Users.Queries.GetUserByIdQuery;
using Application.Features.Users.Queries.LoginUserQuery;
using MediatR;
using WebApi.Common.Filters;
using UserDto = Application.Features.Users.Queries.GetAllUsersQuery.UserDto;

namespace WebApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("api/users").WithOpenApi()
                                            .WithTags("Users")
                                            .RequireAuthorization();
        
        #region GET
        
        root.MapGet("/", async (IMediator mediator) => 
        {
            return Results.Ok(await mediator.Send(new GetAllUsersQuery()));
        }).Produces<BaseResponse<IEnumerable<UserDto>>>();

        
        root.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetUserByIdQuery(id));
            
            return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
        }).Produces<BaseResponse<GetUserByIdDto>>();
        
        #endregion
        
        
        #region POST
        
        root.MapPost("/login", async (IMediator mediator, LoginUserQuery query) =>
        {
            var loginResult = await mediator.Send(query);
            return loginResult.IsSuccess ? Results.Ok(loginResult) : Results.BadRequest(loginResult);
        }).AllowAnonymous().Produces<BaseResponse<string>>();

        
        root.MapPost("/register", async (IMediator mediator, RegisterUserCommand command) =>
        {
            var registerResult = await mediator.Send(command);

            return registerResult.IsSuccess ? Results.Ok(registerResult) 
                                            : Results.BadRequest(registerResult);
        }).AddEndpointFilter<ValidationFilter<RegisterUserCommand>>().Produces<BaseResponse<Guid>>();

        #endregion
        

        #region PUT
        
        root.MapPut("/", async (IMediator mediator, UpdateUserCommand command) =>
        {
            var response = await mediator.Send(command);

            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AddEndpointFilter<ValidationFilter<UpdateUserCommand>>().Produces<BaseResponse<UserDto>>();
        
        #endregion
        
        
        #region DELETE
        
        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new DeleteUserCommand(id));

            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        });
        
        #endregion
    }
}