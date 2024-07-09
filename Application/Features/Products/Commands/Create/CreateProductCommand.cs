using Application.Common.Services;
using Application.Features.Products.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using FluentValidation;
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
    private readonly IUnitOfWork uow;
    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;
    private readonly IValidator<CreateProductCommand> validator;
    private readonly IAuthService authService;

    public CreateProductCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService, IValidator<CreateProductCommand> validator)
    {
        this.authService = authService;
        this.uow = uow;
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
        this.validator = validator;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.CreatedAt = DateTime.UtcNow;
        product.CreatedUserId = authService.GetUserId();

        await productRepo.InsertAsync(product);
        await uow.SaveChangesAsync();

        return mapper.Map<ProductDto>(product);
    }
}