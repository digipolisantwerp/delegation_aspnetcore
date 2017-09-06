using System.Linq;

namespace Digipolis.Delegation.Utilities
{
    public static class ClaimExtensions
    {
        public static string GetClaimValue(this System.Security.Claims.ClaimsIdentity claimIdentity, string claimName)
        {
            if (claimIdentity?.HasClaim(c => c.Type == claimName) == true)
            {
                return claimIdentity.Claims.FirstOrDefault(claim => claim.Type == claimName).Value;
            }
              
            return string.Empty;
        }

    }
}
