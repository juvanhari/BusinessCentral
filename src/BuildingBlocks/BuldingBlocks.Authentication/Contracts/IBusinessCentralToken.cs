namespace BuldingBlocks.Authentication.Contracts
{
    public interface IBusinessCentralToken
    {
        Task<string> GetAuthToken();
    }
}
