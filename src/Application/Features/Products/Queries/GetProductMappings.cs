using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Queries;

public class GetProductMappings : Profile
{
    public GetProductMappings()
    {
        CreateMap<Product, ProductDto>();
    }
}