using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Digipolis.Delegation.Jwt;
using Digipolis.Delegation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

        public async Task Invoke(HttpContext context, 
                                 ISecurityTokenValidator tokenValidator, 
                                 IOptions<DelegationOptions> options,
                                 ITokenValidationParametersFactory tokenValidationParametersFactory)
        {
            if (tokenValidator == null) throw new ArgumentNullException(nameof(tokenValidator), $"{nameof(tokenValidator)} cannot be null.");
            if (tokenValidationParametersFactory == null) throw new ArgumentNullException(nameof(tokenValidationParametersFactory), $"{nameof(tokenValidationParametersFactory)} cannot be null.");
                        
            var validationParameters = tokenValidationParametersFactory.Create();

            // get DelegationUser added as scoped service
            var delegationUser = context.RequestServices.GetService(typeof(IDelegationUser)) as DelegationUser;
            bool delegationUserParsed = false;
            var token = string.Empty;

            try
            {
                token = GetDelegationJwtToken(context, options.Value.DelegationHeader);
                
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

                        if (claimsIdentity != null && delegationUser.TrySetValues(claimsIdentity, token)) delegationUserParsed = true;
                        
                        //context.Request.Headers.Add(options.Value.DelegationHeader, "Bearer " + token);
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
                var info = $"No delegated user detected for request { (string.IsNullOrWhiteSpace(token) ? "(no delegation token found)" : $"(delegation token: {token})") }.";                   
                _logger.LogInformation(info);
            }
            
            await _next.Invoke(context);
        }

        public static string GetDelegationJwtToken(HttpContext context, string headerKey)
        {            
            if (context.Request.Headers.ContainsKey(headerKey))
            {
                string header = context.Request.Headers[headerKey];

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
