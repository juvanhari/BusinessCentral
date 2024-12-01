namespace BuldingBlocks.Authentication.Base
{
    public abstract class TokenBase(IOptions<Settings> config)
    {
        public async Task<string> GetAuthToken()
        {
            var authority = $"{config.Value.Instance}{config.Value.TenantId}";

            var app = ConfidentialClientApplicationBuilder
                                 .Create(config.Value.ClientId)
                                 .WithClientSecret(config.Value.ClientSecret)
                                 .WithAuthority(new Uri(authority))
                                 .Build();

            var result = await app.AcquireTokenForClient(new[] { config.Value.Resource + "/.default" })
                                                 .ExecuteAsync();

            return result.AccessToken;
        }
    }
}
