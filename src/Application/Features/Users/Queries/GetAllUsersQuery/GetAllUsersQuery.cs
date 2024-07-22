using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsersQuery;

public class GetAllUsersQuery : IRequest<BaseResponse<IEnumerable<UserDto>>>
{

}