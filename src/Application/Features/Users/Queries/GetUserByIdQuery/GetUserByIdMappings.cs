using Application.Features.Products.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdMappings : Profile
{
    public GetUserByIdMappings()
    {
        CreateMap<Product, ProductDto>();
    }
}