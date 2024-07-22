using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdQuery : IRequest<BaseResponse<GetUserByIdDto>>
{
    public GetUserByIdQuery(Guid id) => Id = id;
    public Guid Id { get; }
}