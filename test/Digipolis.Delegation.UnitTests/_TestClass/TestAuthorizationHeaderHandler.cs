using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipolis.Delegation.Handlers;
using Digipolis.Delegation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Digipolis.Delegation.UnitTests._TestClass
{
    public class TestAuthorizationHeaderHandler : AuthorizationHeaderHandler
    {
        public TestAuthorizationHeaderHandler(ILogger<AuthorizationHeaderHandler> logger, IOptions<DelegationOptions> options, IHttpContextAccessor accessor) : base(logger, options, accessor)
        {
            InnerHandler = Mock.Of<HttpMessageHandler>();
        }

        public async Task<HttpResponseMessage> TestSendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await SendAsync(request, cancellationToken);
        }
    }
}