namespace Msdbc.Api.Features.Catalog.GetDiscount
{
    public record GetDiscountByProductIdRequest(Guid Id);

    public record GetDiscountByProductIdResponse(int Discount);


    public class GetDiscountByProductIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/discounts/{productId}", async (Guid productId, ISender sender) =>
            {
                var result = await sender.Send(new GetDiscounttByProductIdQuery(productId));

                var response = result.Adapt<GetDiscountByProductIdResponse>();

                return Results.Ok(response.Discount);
            })
            .WithName("GetDiscountByProductId")
            .Produces<GetDiscountByProductIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Discount By Product Id")
            .WithDescription("Get Discount By Product Id");
        }
    }
}
