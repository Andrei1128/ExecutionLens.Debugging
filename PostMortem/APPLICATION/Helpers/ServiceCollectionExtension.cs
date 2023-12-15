using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using PostMortem.APPLICATION.Contracts;
using PostMortem.APPLICATION.Implementations;
using PostMortem.DOMAIN.Utilities;

namespace PostMortem.APPLICATION.Helpers;
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
                var logInterceptor = provider.GetRequiredService<IInterceptorService>();
                var implementationInstance = provider.GetRequiredService<TImplementation>();
                TService? proxy = null;
                proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, logInterceptor);
                return proxy;
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
        services.AddScoped<StructuredLoggingAttribute>();
        return new LoggerConfiguration();
    }
}