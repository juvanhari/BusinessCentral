namespace BC.Api.Features.Authentication.GetToken
{
    public record GetAuthTokenCommand() : ICommand<GetAuthTokenResult>;

    public record GetAuthTokenResult(string Token);

    public class GetAuthTokenHandler(IBusinessCentralToken businessCentralToken) : ICommandHandler<GetAuthTokenCommand, GetAuthTokenResult>
    {
        public async Task<GetAuthTokenResult> Handle(GetAuthTokenCommand command, CancellationToken cancellationToken)
        {
            // Fetch the token
            var token = await businessCentralToken.GetAuthToken();
            return new GetAuthTokenResult(token);
        }
    }
}
