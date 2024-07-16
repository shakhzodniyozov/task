using Application;
using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Queries;
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
        }).Produces<IEnumerable<ProductDto>>();

        
        root.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetProductByIdQuery(id));

            return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Errors);
        }).Produces<ProductDto>();

        #endregion
        
        
        #region POST
        
        root.MapPost("/", async (IMediator mediator, CreateProductCommand command) =>
        {
            var createResult = await mediator.Send(command);

            return createResult.IsSuccess
                ? Results.Ok(createResult.Value)
                : Results.BadRequest(createResult.Reasons);
        }).AddEndpointFilter<ValidationFilter<CreateProductCommand>>().Produces<ProductDto>();
        
        #endregion

        
        #region PUT
        
        root.MapPut("/", async (IMediator mediator, UpdateProductCommand command) =>
        {
            var updateResult = await mediator.Send(command);

            return updateResult.IsSuccess ? Results.Ok(updateResult.Value) : Results.BadRequest(updateResult.Reasons);
        }).AddEndpointFilter<ValidationFilter<UpdateProductCommand>>().Produces<ProductDto>();
        
        #endregion

        
        #region DELETE
        
        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var deleteResult = await mediator.Send(new DeleteProductCommand(id));

            return deleteResult.IsSuccess ? Results.NoContent() : Results.BadRequest(deleteResult.Reasons);
        });
        
        #endregion
    }
}