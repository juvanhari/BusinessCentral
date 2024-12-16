namespace BC.Api.Features.Catalog.Command.UpdateProduct
{
    public record UpdateProductRequest(ProductCommandDto Product);
    public record UpdateProductResponse(ProductQueryDto Product);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{Id}", async (Guid Id, UpdateProductRequest request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateProductCommand(Id, request.Product));

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Created($"/products/{response.Product.Id}", response);
            })
             .RequireAuthorization() // Enforce Authorization
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
        }
    }
}
