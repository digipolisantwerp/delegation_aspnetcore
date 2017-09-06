using Digipolis.Delegation.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Digipolis.Delegation
{
    internal class DelegationMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<DelegationMiddleware> _logger;
        
        public DelegationMiddleware(RequestDelegate next, ILogger<DelegationMiddleware> logger)
        {
            if (next == null) throw new ArgumentNullException(nameof(next), $"{nameof(next)} cannot be null.");
            if (logger == null) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null.");
            
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ISecurityTokenValidator tokenValidator, 
                                 ITokenValidationParametersFactory tokenValidationParametersFactory)
        {
            if (tokenValidator == null) throw new ArgumentNullException(nameof(tokenValidator), $"{nameof(tokenValidator)} cannot be null.");
            if (tokenValidationParametersFactory == null) throw new ArgumentNullException(nameof(tokenValidationParametersFactory), $"{nameof(tokenValidationParametersFactory)} cannot be null.");
                        
            var validationParameters = tokenValidationParametersFactory.Create();

            // get DelegationUser added as scoped service
            var delegationUser = context.RequestServices.GetService(typeof(IDelegationUser)) as DelegationUser;
            bool delegationUserParsed = false;

            try
            {
                var token = GetDelegationJwtToken(context);
                
                if (!string.IsNullOrWhiteSpace(token))
                {
                    if (tokenValidator.CanReadToken(token))
                    {
                        ClaimsPrincipal principal = null;
                        SecurityToken validatedToken = null;

                        try
                        {
                            principal = tokenValidator.ValidateToken(token, validationParameters, out validatedToken);
                            _logger.LogInformation($"Jwt delegation token validation succeeded");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"Jwt delegation token validation failed. Exception: {ex.ToString()}");
                            throw;
                        }

                        ClaimsIdentity claimsIdentity = principal.Identities?.FirstOrDefault();

                        if (claimsIdentity != null && delegationUser.TrySetValues(claimsIdentity)) delegationUserParsed = true;
                    }
                } 
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Processing delegation user failed. Exception: {ex.ToString()}");
                throw;
            }

            if (delegationUserParsed)
            {
                delegationUser.SetValid(true);
                _logger.LogInformation($"Request for delegated user: { delegationUser.Sub} ({delegationUser.GivenName} {delegationUser.SurName})");
            }
            else
            {
                delegationUser.SetValid(false);
                _logger.LogInformation($"No delegated user detected for request.");
            }
            
            await _next.Invoke(context);
        }

        private string GetDelegationJwtToken(HttpContext context)
        {            
            if (context.Request.Headers.ContainsKey(HeaderKeys.Delegation))
            {
                string header = context.Request.Headers[HeaderKeys.Delegation];

                if (!string.IsNullOrWhiteSpace(header))
                {
                    if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        return header.Substring("Bearer ".Length).Trim();
                    }
                    else return header;
                }
            }

            return string.Empty;
        }
    }
}
