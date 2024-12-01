namespace BuldingBlocks.Authentication.Base
{
    public abstract class Settings : ISettings
    {
        public string Instance { get; set; } = default!;

        public string ClientId { get; set; } = default!;

        public string ClientSecret { get; set; } = default!;


        public string TenantId { get; set; } = default!;

        public string Resource { get; set; } = default!;
    }
}
