using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Features.Users.Queries;
using Application.Features.Users.Queries.GetAllUsersQuery;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseResponse<UserDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteUserCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<UserDto>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user is null)
            return new ErrorResponse<UserDto>($"User with provided Id={request.Id} was not found.");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChanges(cancellationToken);

        return new SuccessResponse<UserDto>(null!);
    }
}