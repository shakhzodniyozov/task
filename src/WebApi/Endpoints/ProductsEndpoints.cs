using Application.Common.Responses;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries;
using Application.Features.Products.Queries.GetAllProductsQuery;
using Application.Features.Products.Queries.GetProductByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;

namespace WebApi.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication webApplication)
    {
        var root = webApplication.MapGroup("api/products").WithOpenApi()
            .WithTags("Products")
            .RequireAuthorization();

        #region GET

        root.MapGet("/", async ([FromServices] IMediator mediator) =>
        {
            var response = await mediator.Send(new GetAllProductsQuery());
            return Results.Ok(response);
        }).Produces<BaseResponse<IEnumerable<ProductDto>>>();


        root.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetProductByIdQuery(id));

            return response is SuccessResponse<ProductDto> ? Results.Ok(response) : Results.NotFound(response);
        }).Produces<BaseResponse<ProductDto>>();

        #endregion


        #region POST

        root.MapPost("/", async (IMediator mediator, CreateProductCommand command) =>
        {
            var createResult = await mediator.Send(command);

            return Results.Ok(createResult);
        }).AddEndpointFilter<ValidationFilter<CreateProductCommand>>().Produces<BaseResponse<ProductDto>>();

        #endregion


        #region PUT

        root.MapPut("/", async (IMediator mediator, UpdateProductCommand command) =>
        {
            var updateResult = await mediator.Send(command);

            return updateResult.IsSuccess ? Results.Ok(updateResult) : Results.BadRequest(updateResult);
        }).AddEndpointFilter<ValidationFilter<UpdateProductCommand>>().Produces<BaseResponse<ProductDto>>();

        #endregion


        #region DELETE

        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var deleteResult = await mediator.Send(new DeleteProductCommand(id));

            return deleteResult.IsSuccess ? Results.Ok(deleteResult) : Results.BadRequest(deleteResult);
        });

        #endregion
    }
}