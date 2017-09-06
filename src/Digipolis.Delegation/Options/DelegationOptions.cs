namespace Digipolis.Delegation.Options
{
    public class DelegationOptions
    {
        /// <summary>
        /// The name of the delegation header.
        /// </summary>
        public string DelegationHeader { get; set; } = DelegationOptionsDefaults.DelegationHeader;

        /// <summary>
        /// The name of the application in which context the user is requesting a resource.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The base url for the application, including scheme and eventual port.
        /// ex. https://test.antwerpen.be:443
        /// </summary>
        public string ApplicationBaseUrl { get; set; }
        
        /// <summary>
        /// The audience url used to validate the Jwt token.
        /// </summary>
        public string JwtAudience { get; set; }

        /// <summary>
        /// The issuer string used to validate the Jwt token.
        /// </summary>
        public string JwtIssuer { get; set; }

        /// <summary>
        /// The duration in minutes the Jwt signing key is cached.
        /// Default = 1440 minutes (24 hours).
        /// </summary>
        public int JwtSigningKeyCacheDuration { get; set; } = 1440;

    }
}
