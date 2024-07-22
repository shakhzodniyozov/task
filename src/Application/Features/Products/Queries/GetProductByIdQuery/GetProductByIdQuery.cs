using Application.Common.Responses;
using MediatR;

namespace Application.Features.Products.Queries.GetProductByIdQuery;

public class GetProductByIdQuery : IRequest<BaseResponse<ProductDto>>
{
    public GetProductByIdQuery(Guid id) => Id = id;

    public Guid Id { get; set; }
}