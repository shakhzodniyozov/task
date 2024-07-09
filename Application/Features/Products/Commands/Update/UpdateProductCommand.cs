using Application.Common.Services;
using Application.Features.Products.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MediatR;

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
    private readonly IUnitOfWork uow;
    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public UpdateProductCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService)
    {
        this.uow = uow;
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
        this.authService = authService;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id, disableTracking: false);

        if (product is null)
            return Result.Fail($"Product with provided Id={request.Id} was not found.");
        
        mapper.Map(request, product);
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdateUserId = authService.GetUserId();

        await uow.SaveChangesAsync();

        return Result.Ok(mapper.Map<ProductDto>(product));
    }
}