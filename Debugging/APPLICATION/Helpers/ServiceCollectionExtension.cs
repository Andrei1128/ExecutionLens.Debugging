using Debugging.APPLICATION.Contracts;
using Debugging.APPLICATION.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Debugging.APPLICATION.Helpers;
public static partial class ServiceCollectionExtensions
{
    public static void AddDebugger(this IServiceCollection services)
    {
        services.AddScoped<IReflectionService, ReflectionService>();
    }
}