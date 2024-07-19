using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductMappings : Profile
{
    public UpdateProductMappings()
    {
        CreateMap<UpdateProductCommand, Product>();
    }
}