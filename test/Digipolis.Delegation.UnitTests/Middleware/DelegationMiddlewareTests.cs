using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Digipolis.Delegation;
using Digipolis.Delegation.Jwt;
using Digipolis.Delegation.Options;
using Digipolis.Delegation.UnitTests._TestUtilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace Digipolis.Delegation.UnitTests.Middleware
{
    public class DelegationMiddlewareTests
    {

        private const string Sub = "MySub";
        private const string RRNr = "MyRRNR";
        private const string XCredentialUserName = "MyXCredentialUserName";
        private const string Name = "MyName";
        private const string GivenName = "MyGivenName";
        private const string SurName = "MySurName";
        private const string ProfileId = "MyProfileId";
        private const string ProfileType = "MyProfileType";
        private const string Token = "MyToken";
        private const string HeaderName = "MyHeader";

        [Fact]
        public async Task ShouldSetDelegationUser()
        {
            var headers = new Dictionary<string, StringValues>() {
                { HeaderName, $"Bearer {Token}" }
            };

            var user = new DelegationUser();
            
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(x => x.Request.Headers)
                .Returns(new HeaderDictionary(headers));
            httpContext
                .Setup(x => x.Items)
                .Returns(new Dictionary<object, object>());
            httpContext
                .Setup(x => x.RequestServices.GetService(typeof(IDelegationUser)))
                .Returns(user);


            var requestDelegate = new RequestDelegate(
                (innerContext) => Task.FromResult(0));

            var logger = TestLogger<DelegationMiddleware>.CreateLogger();
            
            var middleware = new DelegationMiddleware(requestDelegate, logger);

            var options = TestOptions.Create(new DelegationOptions
            {
                DelegationHeader = HeaderName
            });
            var tokenValidationParametersFactory = new TokenValidationParametersFactory(
                options,
                Mock.Of<IJwtSigningKeyResolver>(),
                Mock.Of<IHostingEnvironment>());
            
            var tokenValidator = new Mock<ISecurityTokenValidator>();
            tokenValidator
                .Setup(x => x.CanReadToken(It.IsAny<string>()))
                .Returns(true);

            var token =  new Mock<SecurityToken>().Object;
            
            var claim = new ClaimsPrincipal(new []
            {
                new ClaimsIdentity(new []
                {
                    new Claim(Claims.Sub, Sub),
                    new Claim(Claims.RRNr, RRNr),
                    new Claim(Claims.XCredentialUserName, XCredentialUserName),
                    new Claim(Claims.Name, Name),
                    new Claim(Claims.GivenName, GivenName),
                    new Claim(Claims.SurName, SurName),
                    new Claim(Claims.ProfileId, ProfileId),
                    new Claim(Claims.ProfileType, ProfileType),
                }), 
            });

            tokenValidator
                .Setup(x => x.ValidateToken(
                    It.IsAny<string>(), 
                    It.IsAny<TokenValidationParameters>(),
                    out token))
                .Returns(claim);
            
            await middleware.Invoke(httpContext.Object, 
                tokenValidator.Object, 
                options,
                tokenValidationParametersFactory);

            Assert.Equal(Sub, user.Sub);
            Assert.Equal(RRNr, user.RRNr);
            Assert.Equal(XCredentialUserName, user.XCredentialUserName);
            Assert.Equal(Name, user.Name);
            Assert.Equal(GivenName, user.GivenName);
            Assert.Equal(SurName, user.SurName);
            Assert.Equal(ProfileId, user.ProfileId);
            Assert.Equal(ProfileType, user.ProfileType);
            Assert.Equal(Token, user.JwtToken);

        }
    }
}