using Microsoft.IdentityModel.Tokens;

namespace Digipolis.Delegation.Jwt
{
    public interface ITokenValidationParametersFactory
    {
        TokenValidationParameters Create();
    }
}
