using Application.Features.Products.Queries;
using FluentResults;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Result<ProductDto>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}