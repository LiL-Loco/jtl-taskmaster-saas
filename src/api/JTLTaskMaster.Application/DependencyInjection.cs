using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JTLTaskMaster.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here
        // Example: services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}