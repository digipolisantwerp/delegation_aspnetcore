using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Digipolis.Delegation.Handlers;
using Digipolis.Delegation.Jwt;
using Digipolis.Delegation.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Digipolis.Delegation
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds delegation services to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDelegation(this IServiceCollection services, Action<DelegationOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} cannot be null.");

            services.Configure(setupAction);

            RegisterServices(services);

            return services;
        }
        
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IDelegationUser, DelegationUser>();

            services.AddSingleton<IJwtSigningKeyResolver, JwtSigningKeyResolver>();
            services.AddSingleton<ISecurityTokenValidator, JwtSecurityTokenHandler>();
            services.AddSingleton<ITokenValidationParametersFactory, TokenValidationParametersFactory>();
            
            services.TryAddSingleton<HttpMessageHandler, HttpClientHandler>();
            services.AddTransient<AuthorizationHeaderHandler>();
            services.AddHttpClient(string.Empty)
                .AddHttpMessageHandler<AuthorizationHeaderHandler>();
            
        }
        
    }
}
