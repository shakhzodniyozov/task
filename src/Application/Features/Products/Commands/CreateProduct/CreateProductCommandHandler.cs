using Application.Common.Interfaces;
using Application.Features.Products.Queries;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MassTransit;
using MediatR;
using Shared.Events.Contracts.Product;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IApplicationDbContext _dbContext;

    public CreateProductCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IAuthService authService,
        IPublishEndpoint publishEndpoint)
    {
        _authService = authService;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedUserId = _authService.GetUserId();

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChanges(cancellationToken);

        await _publishEndpoint.Publish(new ProductCreated(product.Id, DateTime.Now), cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }
}