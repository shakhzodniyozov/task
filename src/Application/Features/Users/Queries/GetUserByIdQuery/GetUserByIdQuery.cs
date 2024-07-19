using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public GetUserByIdQuery(Guid id) => Id = id;
    public Guid Id { get; }
}