using Application.Common.Interfaces;
using Application.Features.Users.Queries;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.Update.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public UpdateUserCommandHandler(IApplicationDbContext dbContext, IAuthService authService, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetUserId();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        _mapper.Map(request, user);
        await _dbContext.SaveChanges(cancellationToken);

        return Result.Ok(_mapper.Map<UserDto>(user));
    }
}