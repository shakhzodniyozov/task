using MediatR;

namespace Application.Features.Users.Queries.GetAllUsersQuery;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{

}