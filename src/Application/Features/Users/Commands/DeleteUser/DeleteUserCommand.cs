using Application.Common.Responses;
using Application.Features.Users.Queries;
using Application.Features.Users.Queries.GetAllUsersQuery;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<BaseResponse<UserDto>>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}