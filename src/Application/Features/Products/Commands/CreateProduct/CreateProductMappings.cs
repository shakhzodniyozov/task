using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductMappings : Profile
{
    public CreateProductMappings()
    {
        CreateMap<CreateProductCommand, Product>();
    }
}