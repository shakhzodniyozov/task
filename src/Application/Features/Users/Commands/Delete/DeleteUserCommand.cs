using Application.Common.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<Result>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteUserCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user is null)
            return Result.Fail($"User with provided Id={request.Id} was not found.");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChanges();

        return Result.Ok();
    }
}
