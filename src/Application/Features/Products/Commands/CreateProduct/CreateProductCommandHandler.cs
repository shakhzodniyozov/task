using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Features.Products.Queries;
using AutoMapper;
using Domain.Entities;
using MassTransit;
using MediatR;
using Shared.Events.Contracts.Product;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseResponse<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IApplicationDbContext _dbContext;

    public CreateProductCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IAuthenticationService authenticationService,
        IPublishEndpoint publishEndpoint)
    {
        _authenticationService = authenticationService;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedUserId = _authenticationService.GetUserId();

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChanges(cancellationToken);

        await _publishEndpoint.Publish(new ProductCreated(product.Id, DateTime.Now), cancellationToken);

        return new SuccessResponse<ProductDto>(_mapper.Map<ProductDto>(product));
    }
}