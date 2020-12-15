using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Digipolis.Delegation.Handlers;
using Digipolis.Delegation.Options;
using Digipolis.Delegation.UnitTests._TestClass;
using Digipolis.Delegation.UnitTests._TestUtilities;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Digipolis.Delegation.UnitTests.Handlers
{
    public class AuthorizationHeaderHandlerTests
    {
        
        private const string Token = "MyToken";
        private const string HeaderName = "MyHeader";

        [Fact]
        public async Task ShouldSetHeaderUser()
        {
            var user = new DelegationUser();
            user.TrySetValues(new ClaimsIdentity(), Token);
            
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor
                .Setup(x => x.HttpContext.RequestServices.GetService(typeof(IDelegationUser)))
                .Returns(user);
            
            var logger = TestLogger<AuthorizationHeaderHandler>.CreateLogger();
            
            var options = TestOptions.Create(new DelegationOptions
            {
                DelegationHeader = HeaderName
            });

            var handler = new TestAuthorizationHeaderHandler(logger, options, httpContextAccessor.Object);

            var request = new HttpRequestMessage();
            
            await handler.TestSendAsync(request, CancellationToken.None);

            var token = request.Headers.FirstOrDefault(h => h.Key == HeaderName).Value?.FirstOrDefault();
            Assert.NotNull(token);
            Assert.Equal(Token, token);

        }

    }
}