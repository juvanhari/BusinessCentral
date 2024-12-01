using Microsoft.AspNetCore.Authorization;

namespace BC.Api.Features.Catalog.CreateProduct
{
    public record CreateProductRequest(ProductDto ProductDto);

    public record CreateProductResponse(Guid Id);

    [Authorize]
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
            .RequireAuthorization()
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
