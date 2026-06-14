using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wellway.Infrastructure.Persistence;
using Wellway.Infrastructure.Persistence.Interceptors;

namespace Wellway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<AuditInterceptor>();

        services.AddDbContext<WellwayDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();

            options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(interceptor);
        });

        // TODO: Register ASP.NET Core Identity — wired during auth feature implementation

        return services;
    }
}
