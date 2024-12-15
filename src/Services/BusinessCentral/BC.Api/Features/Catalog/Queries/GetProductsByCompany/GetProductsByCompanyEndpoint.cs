namespace BC.Api.Features.Catalog.Queries.GetProductByCompany
{
    public record GetProductsByCompanyResponse(PaginatedResult<ProductQueryDto> Products);
    public class GetProductsByCompanyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/company/{company}", async ([AsParameters] PaginationRequest request, string company, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCompanyQuery(company, request));

                var response = result.Adapt<GetProductsByCompanyResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductsByCompany")
            .Produces<GetProductsByCompanyResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products By Company")
            .WithDescription("Get Products By Company");
        }
    }
}
