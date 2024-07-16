using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<UpdateProductCommand, Product>();
    }
}