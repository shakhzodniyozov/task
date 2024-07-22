using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Features.Users.Queries;
using Application.Features.Users.Queries.GetAllUsersQuery;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UpdateUser.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<UserDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;

    public UpdateUserCommandHandler(IApplicationDbContext dbContext, IAuthenticationService authenticationService, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    public async Task<BaseResponse<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _authenticationService.GetUserId();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        _mapper.Map(request, user);
        await _dbContext.SaveChanges(cancellationToken);

        return new SuccessResponse<UserDto>(_mapper.Map<UserDto>(user));
    }
}