using MediatR;

namespace Application.Features.Products.Queries.GetAllProductsQuery;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{

}