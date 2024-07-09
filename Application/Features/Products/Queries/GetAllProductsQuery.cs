using Application.Common.Interfaces;
using Application.Features.Products.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{

}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    public GetAllProductsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products.AsNoTracking()
                                                            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ProductDto>>(products);
    }
}