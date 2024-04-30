using Castle.DynamicProxy;
using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.DOMAIN.Extensions;
using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    
    public object CreateInstance(Mock mock, Mock? parent = null)
    {
        
        Type classType = Type.GetType(mock.Class)
            ?? throw new Exception($"Type `{mock.Class}` not found!");

        List<object> dependencies = [];

        foreach (Mock interaction in mock.Interactions)
        {
            object interactionInstance = CreateInstance(interaction, mock);

            dependencies.Add(interactionInstance);
        }

        IEnumerable<Type> constructorParametersTypes = classType.GetConstructorParametersTypes();

        IEnumerable<Type> dummyDependencies = constructorParametersTypes.GetTypesExcluding([.. dependencies]);

        foreach (Type dependency in dummyDependencies)
        {
            if(dependency.IsInterface)
            {
                dependencies.Add(proxyGenerator.CreateInterfaceProxyWithoutTarget(dependency));
            }
            else
            {
                dependencies.Add(proxyGenerator.CreateClassProxy(dependency));
            }
        }

        object instance = Activator.CreateInstance(classType, [.. dependencies])
            ?? throw new Exception($"Could not create instance for `{classType}`!");

        if (parent is null)
        {
            return instance;
        }

        InterceptorService interceptor = new(mock.Setups);

        Type parentClassType = Type.GetType(parent.Class)
            ?? throw new Exception($"Type `{mock.Class}` not found!");

        Type interfaceType = parentClassType.GetConstructorParametersTypes()
                                            .First(p => p.IsAssignableFrom(classType));

        return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, instance, interceptor);
    }
}