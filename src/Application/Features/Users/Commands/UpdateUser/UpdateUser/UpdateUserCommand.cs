using Application.Common.Responses;
using Application.Features.Users.Queries;
using Application.Features.Users.Queries.GetAllUsersQuery;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser.UpdateUser;

public class UpdateUserCommand : IRequest<BaseResponse<UserDto>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}