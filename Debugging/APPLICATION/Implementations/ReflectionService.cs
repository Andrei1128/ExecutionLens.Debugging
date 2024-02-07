using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Extensions;
using Debugging.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(Mock mock, Mock? parent = null)
    {
        Type classType = mock.GetClassType();

        List<object> dependencies = [];

        foreach (var interaction in mock.Interactions)
        {
            dependencies.Add(CreateInstance(interaction, mock));
        }

        IEnumerable<Type> dummyDependencies = GetParametersExcluding(GetConstructorParameters(classType), dependencies);

        foreach (var dependency in dummyDependencies)
        {
            dependencies.Add(proxyGenerator.CreateClassProxy(dependency));
        }

        object instance = Activator.CreateInstance(classType, [.. dependencies])
            ?? throw new Exception($"Could not create instance for '{classType}'!");

        if (parent is null)
        {
            return instance;
        }

        var interceptor = new InterceptorService(mock.Setups);

        var interfaceType = GetConstructorParameters(parent.GetClassType()).FirstOrDefault(p => p.IsAssignableFrom(classType));

        return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, instance, interceptor);
    }

    private IEnumerable<Type> GetParametersExcluding(Type[] parameters, List<object> excludeList)
    {
        return (from param in parameters
                where !excludeList.Any(x => param.IsAssignableFrom(x.GetType()))
                select param) ?? [];
    }

    private Type[] GetConstructorParameters(Type classType)
    {
        ConstructorInfo[] constructors = classType.GetConstructors();

        if (constructors.Length != 0)
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters().Select(x => x.ParameterType).ToArray();
        }

        return [];
    }
}