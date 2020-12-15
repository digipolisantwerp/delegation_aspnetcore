using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Digipolis.ApplicationServices;
using Digipolis.Auth;
using Digipolis.Auth.Constants;
using Example;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Digipolis.Delegation.Integration.Tests.TestClasses
{
    public abstract class BaseTestController : IClassFixture<ExampleAppFactory>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        protected HttpClient Client;

        protected BaseTestController(ExampleAppFactory factory)
        {
            Factory = factory;
            SetupClient();
        }

        protected async Task<T> GetData<T>(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                } 
            }
            
            
            var response = await Client.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        private void SetupClient()
        {
            Client = Factory.WithWebHostBuilder(builder =>
            {

                builder.ConfigureTestServices(services =>
                {
                    services.AddApplicationServices(setup =>
                    {
                        setup.ApplicationName = Constants.OrganisationId;
                        setup.ApplicationId = Constants.ApplicationId;
                    });

                    services.AddAuthFromOptions(options =>
                    {
                        options.PdpUrl = Constants.BraasEndpoint;
                        options.PdpCacheDuration = 0;
                        options.MeAuthzUrl = Constants.MeAuthzUrl;
                        options.ApiKey = Constants.ApiKey;
                        options.JwtTokenSource = "header";
                        options.ApplicationName = Constants.ApplicationId;
                        options.PermissionSource = Constants.PermissionSource;
                    });
            
                    var customPolicy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(AuthScheme.JwtHeaderAuth)
                        .RequireAuthenticatedUser()
                        .Build();

                    services.AddAuthorizationCore(services.BuildJwtAuthPolicies(
                        new Dictionary<string, AuthorizationPolicy>
                        {
                            {"Authenticated", customPolicy}
                        }));
            
                    services.AddControllers();
                    services.AddDelegation(options =>
                    {
                        options.ApplicationName = Constants.ApplicationId;
                    });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}