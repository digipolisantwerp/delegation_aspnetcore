using Microsoft.AspNetCore.Builder;

namespace Digipolis.Delegation
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseDelegation(this IApplicationBuilder app)
        {
            app.UseMiddleware<DelegationMiddleware>();

            return app;
        }
    }
}
