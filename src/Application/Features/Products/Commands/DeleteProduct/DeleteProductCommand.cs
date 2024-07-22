using Application.Common.Responses;
using Application.Features.Products.Queries;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<BaseResponse<ProductDto>>
{
    public DeleteProductCommand(Guid id) => Id = id;

    public Guid Id { get; set; }
}
