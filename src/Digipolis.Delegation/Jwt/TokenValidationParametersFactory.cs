using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Digipolis.Delegation.Jwt
{
    public class TokenValidationParametersFactory : ITokenValidationParametersFactory
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IJwtSigningKeyResolver _jwtSigningKeyProvider;

        public TokenValidationParametersFactory(IJwtSigningKeyResolver jwtSigningKeyProvider,
                                                IHostingEnvironment  hostingEnvironment)
        {
            if (jwtSigningKeyProvider == null) throw new ArgumentNullException(nameof(jwtSigningKeyProvider), $"{nameof(jwtSigningKeyProvider)} cannot be null");
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment), $"{nameof(hostingEnvironment)} cannot be null");
            
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
                ValidIssuer = string.Empty,
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
