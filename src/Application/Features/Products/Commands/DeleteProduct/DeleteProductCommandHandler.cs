using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, BaseResponse<ProductDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteProductCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<ProductDto>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return new ErrorResponse<ProductDto>($"Product with provided Id={request.Id} was not found.");
        }
        
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChanges(cancellationToken);
        return new SuccessResponse<ProductDto>(null!);
    }
}