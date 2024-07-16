using Application.Common.Interfaces;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public GetUserByIdQuery(Guid id) => Id = id;
    public Guid Id { get; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return user is not null ? Result.Ok(_mapper.Map<UserDto>(user)) : Result.Fail<UserDto>($"User with provided Id={request.Id} was not found.");
    }
}
