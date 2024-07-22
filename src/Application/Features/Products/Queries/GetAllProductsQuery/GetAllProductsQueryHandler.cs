using Application.Common.Interfaces;
using Application.Common.Responses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetAllProductsQuery;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, BaseResponse<IEnumerable<ProductDto>>>
{
    public GetAllProductsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public async Task<BaseResponse<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products.AsNoTracking()
            .ToListAsync(cancellationToken);

        return new SuccessResponse<IEnumerable<ProductDto>>(_mapper.Map<List<ProductDto>>(products));
    }
}