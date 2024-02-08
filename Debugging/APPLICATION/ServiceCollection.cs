using PostMortem.Debugging.APPLICATION.Contracts;
using PostMortem.Debugging.APPLICATION.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace PostMortem.Debugging;

public static partial class ServiceCollection
{
    public static void AddDebugger(this IServiceCollection services)
    {
        services.AddScoped<IReflectionService, ReflectionService>();
    }
}