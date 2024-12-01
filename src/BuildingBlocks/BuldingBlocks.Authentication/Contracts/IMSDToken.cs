namespace BuldingBlocks.Authentication.Contracts
{
    public interface IMSDToken
    {
        Task<string> GetAuthToken();
    }
}
