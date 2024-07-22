using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Features.Products.Queries;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Events.Contracts.Product;

namespace Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, BaseResponse<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IApplicationDbContext _dbContext;

    public UpdateProductCommandHandler(IApplicationDbContext dbContext,
        IMapper mapper,
        IAuthenticationService authenticationService,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<BaseResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            return new ErrorResponse<ProductDto>($"Product with provided Id={request.Id} was not found.");

        _mapper.Map(request, product);
        product.UpdateUserId = _authenticationService.GetUserId();

        await _dbContext.SaveChanges(cancellationToken);
        await _publishEndpoint.Publish(new ProductUpdated(product.Id, DateTime.Now), cancellationToken);

        return new SuccessResponse<ProductDto>(_mapper.Map<ProductDto>(product));
    }
}