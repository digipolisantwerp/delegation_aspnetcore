using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Digipolis.Delegation.Jwt
{
    public interface IJwtSigningKeyResolver
    {
        IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
    }
}
