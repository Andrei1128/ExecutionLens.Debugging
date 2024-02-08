using Castle.DynamicProxy;
using PostMortem.Debugging.APPLICATION.Contracts;
using PostMortem.Debugging.DOMAIN.Extensions;
using PostMortem.Debugging.DOMAIN.Models;
using System.Reflection;

namespace PostMortem.Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(Mock mock, Mock? parent = null)
    {
        Type classType = mock.GetClassType();

        List<object> dependencies = [];

        foreach (Mock interaction in mock.Interactions)
        {
            dependencies.Add(CreateInstance(interaction, mock));
        }

        IEnumerable<Type> dummyDependencies = GetTypesExcluding(GetConstructorParametersTypes(classType), dependencies);

        foreach (Type dependency in dummyDependencies)
        {
            dependencies.Add(proxyGenerator.CreateClassProxy(dependency));
        }

        object instance = Activator.CreateInstance(classType, [.. dependencies])
            ?? throw new Exception($"Could not create instance for '{classType}'!");

        if (parent is null)
        {
            return instance;
        }

        InterceptorService interceptor = new(mock.Setups);

        Type interfaceType = GetConstructorParametersTypes(parent.GetClassType()).First(p => p.IsAssignableFrom(classType));

        return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, instance, interceptor);
    }

    private IEnumerable<Type> GetTypesExcluding(IEnumerable<Type> types, List<object> excluding)
    {
        IEnumerable<Type> excludingTypes =
            from type in types
            where !excluding.Any(x => type.IsAssignableFrom(x.GetType()))
            select type;

        return excludingTypes ?? [];
    }

    private IEnumerable<Type> GetConstructorParametersTypes(Type classType)
    {
        ConstructorInfo[] constructors = classType.GetConstructors();

        if (constructors.Length != 0)
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters().Select(x => x.ParameterType);
        }

        return [];
    }
}