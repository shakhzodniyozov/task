using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.DTOs;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterUserCommand, User>();
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<UpdateProductCommand, Product>();
    }
}
