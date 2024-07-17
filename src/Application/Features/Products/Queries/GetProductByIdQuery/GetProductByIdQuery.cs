using FluentResults;
using MediatR;

namespace Application.Features.Products.Queries.GetProductByIdQuery;

public class GetProductByIdQuery : IRequest<Result<ProductDto>>
{
    public GetProductByIdQuery(Guid id) => Id = id;

    public Guid Id { get; set; }
}