using System.Diagnostics;
using OrganizationsWaterSupplyL4.Data;

namespace OrganizationsWaterSupplyL4.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next) => _next = next;
        public Task Invoke(HttpContext context, IServiceProvider serviceProvider, OrganizationsWaterSupplyContext dbContext)
        {
            try
            {
                UsersDbInitializer.Initialize(context).Wait();
                DbInitializer.Initialize(dbContext);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return _next.Invoke(context);
        }
    }
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }
    }
}
