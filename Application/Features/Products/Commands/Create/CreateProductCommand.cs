using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.Products.Queries;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Products.Commands.Create;

public class CreateProductCommand : IRequest<Result<ProductDto>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly IApplicationDbContext _dbContext;

    public CreateProductCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IAuthService authService)
    {
        _authService = authService;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedUserId = _authService.GetUserId();

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChanges();

        return _mapper.Map<ProductDto>(product);
    }
}