
namespace BC.Api.Features.Authentication.GetToken
{
    public record GetAuthTokenResponse(string token);

    public class GetAuthTokenEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/token", async (ISender sender) =>
            {
                var command = new GetAuthTokenCommand();

                var result = await sender.Send(command);

                var response = result.Adapt<GetAuthTokenResponse>();

                return Results.Ok(response);
            })
            .WithName("GetAuthToken")
            .Produces<GetAuthTokenResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Auth Token")
            .WithDescription("Get Auth Token");
        }
    }
}
