using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<Result>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}