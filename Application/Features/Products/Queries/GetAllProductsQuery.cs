using Application.Features.Products.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{
    public GetAllProductsQuery(int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    public GetAllProductsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
    }

    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepo.GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize);

        return mapper.Map<IEnumerable<ProductDto>>(products.Items);
    }
}