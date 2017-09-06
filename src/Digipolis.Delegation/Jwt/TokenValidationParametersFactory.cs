using System;
using Digipolis.Delegation.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Digipolis.Delegation.Jwt
{
    public class TokenValidationParametersFactory : ITokenValidationParametersFactory
    {
        private readonly DelegationOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IJwtSigningKeyResolver _jwtSigningKeyProvider;

        public TokenValidationParametersFactory(IOptions<DelegationOptions> options, 
                                                IJwtSigningKeyResolver jwtSigningKeyProvider,
                                                IHostingEnvironment  hostingEnvironment)
        {

            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null");
            if (jwtSigningKeyProvider == null) throw new ArgumentNullException(nameof(jwtSigningKeyProvider), $"{nameof(jwtSigningKeyProvider)} cannot be null");
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment), $"{nameof(hostingEnvironment)} cannot be null");

            _options = options.Value;
            _jwtSigningKeyProvider = jwtSigningKeyProvider;
            _hostingEnvironment = hostingEnvironment;
        }

        public TokenValidationParameters Create()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = string.Empty,
                ValidateIssuer = false,
                ValidIssuer = _options.JwtIssuer,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                NameClaimType = "sub",
                RequireSignedTokens = ShouldRequireSignedTokens(),
                IssuerSigningKeyResolver = _jwtSigningKeyProvider.IssuerSigningKeyResolver,
            };
            
            return tokenValidationParameters;
        }

        private bool ShouldRequireSignedTokens()
        {
            var requireSignedTokens = true;
            return requireSignedTokens;
        }
    }
}
