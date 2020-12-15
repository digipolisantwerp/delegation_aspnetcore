using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Digipolis.ApplicationServices;
using Digipolis.Auth;
using Digipolis.Auth.Constants;
using Digipolis.Delegation;
using Digipolis.Delegation.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Example", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example v1"));
                
            }
            app.UseDelegation();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            
        }
    }
}