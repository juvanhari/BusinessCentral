﻿namespace BuldingBlocks.Authentication.Contracts
{
    public interface ISettings
    {
        public string Instance { get; set; }
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }


        public string TenantId { get; set; }

        public string Resource { get; set; }

    }
}
