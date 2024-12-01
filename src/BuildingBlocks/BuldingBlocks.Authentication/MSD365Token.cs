namespace BuldingBlocks.Authentication
{
    public class MSD365Token(IOptions<MSD365Settings> config) : TokenBase(config!), IMSDToken
    {
        public async Task<string> GetAuthToken()
        {
            return await base.GetAuthToken();

        }
    }
}
