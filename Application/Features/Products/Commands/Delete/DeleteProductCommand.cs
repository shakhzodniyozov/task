using Arch.EntityFrameworkCore.UnitOfWork;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<Result>
{
    public DeleteProductCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<Product> productRepo;

    public DeleteProductCommandHandler(IUnitOfWork uow)
    {
        this.uow = uow;
        productRepo = uow.GetRepository<Product>();
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id, disableTracking: false);

        if (product is not null)
        {
            productRepo.Delete(product!);
            await uow.SaveChangesAsync();
            return Result.Ok();
        }
        
        return Result.Fail($"Product with provided Id={request.Id} was not found.");
    }
}
