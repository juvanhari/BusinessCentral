using BuldingBlocks.Authentication.Base;

namespace BuldingBlocks.Authentication
{
    public class BusinessCentralToken(IOptions<BusinessCentralSettings> config) : TokenBase(config!), IBusinessCentralToken
    {
        public async Task<string> GetAuthToken()
        {
            return await base.GetAuthToken();
        }
    }
}
