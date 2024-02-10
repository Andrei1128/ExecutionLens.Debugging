using Castle.DynamicProxy;
using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.APPLICATION.Implementations;
using PostMortem.Logging.DOMAIN.Configurations;
using Microsoft.Extensions.DependencyInjection;
using PostMortem.Logging.DOMAIN.Utilities;

namespace PostMortem.Logging;

public static partial class ServiceCollection
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
            throw new InvalidOperationException($"'{typeof(TService).Name}' has to be a Interface!");

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
        services.AddScoped<LogManager>();
        return new LoggerConfiguration();
    }
}