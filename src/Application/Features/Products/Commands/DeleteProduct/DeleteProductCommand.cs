using FluentResults;
using MediatR;

namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<Result>
{
    public DeleteProductCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}
