using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using PostMortem.LOGGING.APPLICATION.Contracts;
using PostMortem.LOGGING.APPLICATION.Implementations;
using PostMortem.LOGGING.DOMAIN.Utilities;

namespace PostMortem.LOGGING.APPLICATION.Helpers;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggedScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Scoped);

    public static IServiceCollection AddLoggedTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Transient);

    private static IServiceCollection AddLoggedService<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        if (!typeof(TService).IsInterface)
            throw new InvalidOperationException($"`{typeof(TService).Name}` has to be a Interface!");

        services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), lifetime));
        services.Add(ServiceDescriptor.Describe(
            typeof(TService),
            provider =>
            {
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                var interceptorService = provider.GetRequiredService<IInterceptorService>();
                var implementationInstance = provider.GetRequiredService<TImplementation>();
                return proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, interceptorService);
            },
            lifetime
        ));
        return services;
    }
    public static LoggerConfiguration AddLogger(this IServiceCollection services)
    {
        services.AddSingleton<ProxyGenerator>();
        services.AddScoped<IInterceptorService, InterceptorService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IWriteService, WriteService>();
        services.AddScoped<LogAttribute>();
        return new LoggerConfiguration();
    }
}