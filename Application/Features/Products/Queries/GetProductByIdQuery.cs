using Application.Features.Products.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application;

public class GetProductByIdQuery : IRequest<Result<ProductDto>>
{
    public GetProductByIdQuery(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public GetProductByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
    }

    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        return product is not null ? Result.Ok(mapper.Map<ProductDto>(product)) : Result.Fail($"Product with provided Id={request.Id} was not found.");
    }
}