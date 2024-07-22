using Application.Common.Interfaces;
using Application.Common.Responses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetProductByIdQuery;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, BaseResponse<ProductDto>>
{
    public GetProductByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public async Task<BaseResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return product is null
            ? new ErrorResponse<ProductDto>($"Product with provided Id={request.Id} was not found.")
            : new SuccessResponse<ProductDto>(_mapper.Map<ProductDto>(product));
    }
}