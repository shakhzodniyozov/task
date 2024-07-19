using Application.Features.Products.Queries;
using FluentResults;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Result<ProductDto>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}