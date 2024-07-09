using Arch.EntityFrameworkCore.UnitOfWork;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<Result>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<User> userRepo;

    public DeleteUserCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
        userRepo = uow.GetRepository<User>();
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id, disableTracking: false);

        if (user is null)
            return Result.Fail($"User with provided Id={request.Id} was not found.");

        userRepo.Delete(user);
        await uow.SaveChangesAsync();

        return Result.Ok();
    }
}
