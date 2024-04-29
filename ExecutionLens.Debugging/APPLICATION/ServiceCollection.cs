using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.APPLICATION.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionLens.Debugging;

public static partial class ServiceCollection
{
    public static void AddDebugger(this IServiceCollection services)
    {
        services.AddScoped<IReplayService, ReplayService>();
        services.AddScoped<IReflectionService, ReflectionService>();
    }
}