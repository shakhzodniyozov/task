using Application.Common.Responses;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProductsQuery;

public class GetAllProductsQuery : IRequest<BaseResponse<IEnumerable<ProductDto>>>
{

}