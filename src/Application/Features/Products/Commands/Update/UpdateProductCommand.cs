using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.Products.Queries;
using AutoMapper;
using FluentResults;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Events.Contracts.Product;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommand : IRequest<Result<ProductDto>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IApplicationDbContext _dbContext;

    public UpdateProductCommandHandler(IApplicationDbContext dbContext,
        IMapper mapper,
        IAuthService authService,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authService = authService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            return Result.Fail($"Product with provided Id={request.Id} was not found.");

        _mapper.Map(request, product);
        product.UpdateUserId = _authService.GetUserId();

        await _dbContext.SaveChanges(cancellationToken);
        await _publishEndpoint.Publish(new ProductUpdated(product.Id, DateTime.Now), cancellationToken);

        return Result.Ok(_mapper.Map<ProductDto>(product));
    }
}