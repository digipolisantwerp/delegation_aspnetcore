using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipolis.Delegation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digipolis.Delegation.Handlers
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        private readonly ILogger<AuthorizationHeaderHandler> _logger;
        private readonly IOptions<DelegationOptions> _options;
        private readonly IHttpContextAccessor _accessor;
        public AuthorizationHeaderHandler(
            ILogger<AuthorizationHeaderHandler> logger, 
            IOptions<DelegationOptions> options,
            IHttpContextAccessor accessor)
        {
            _logger = logger ?? throw new ArgumentException($"{GetType().Name}.Ctor parameter {nameof(logger)} cannot be null.");
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = String.Empty;
            try
            {
                // retrieve the user from the Request DI Scope, if you use normal DI then it will be a new scope and therefore a new user
                var user = _accessor.HttpContext.RequestServices.GetRequiredService<IDelegationUser>();
                token = user.JwtToken;
            }
            catch (Exception)
            {
                _logger.LogDebug("UserToken not found.");
            }
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add(_options.Value.DelegationHeader, token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}