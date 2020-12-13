using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipolis.Delegation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digipolis.Delegation.Handlers
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        private readonly ILogger<AuthorizationHeaderHandler> _logger;
        private readonly IOptions<DelegationOptions> _options;
        private readonly IDelegationUser _user;
        public AuthorizationHeaderHandler(
            ILogger<AuthorizationHeaderHandler> logger, 
            IOptions<DelegationOptions> options,
            IDelegationUser user)
        {
            _logger = logger ?? throw new ArgumentException($"{GetType().Name}.Ctor parameter {nameof(logger)} cannot be null.");
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = String.Empty;
            try
            {
                token = _user.JwtToken;
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