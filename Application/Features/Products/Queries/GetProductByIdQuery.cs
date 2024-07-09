using Application.Common.Interfaces;
using Application.Features.Products.DTOs;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class GetProductByIdQuery : IRequest<Result<ProductDto>>
{
    public GetProductByIdQuery(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public GetProductByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        return product is not null ? Result.Ok(_mapper.Map<ProductDto>(product)) : Result.Fail($"Product with provided Id={request.Id} was not found.");
    }
}