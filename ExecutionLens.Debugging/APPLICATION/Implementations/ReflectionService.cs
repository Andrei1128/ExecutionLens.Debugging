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

        List<Type> constructorParametersTypes = classType.GetConstructorParametersTypes();

        object[] dependencies = new object[constructorParametersTypes.Count];

        foreach (Mock interaction in mock.Interactions)
        {
            object interactionInstance = CreateInstance(interaction, mock);

            Type interactionType = Type.GetType(interaction.Class)
                ?? throw new Exception($"Type `{interaction.Class}` not found!");

            int index = constructorParametersTypes.GetIndexOf(interactionType);
            dependencies[index] = interactionInstance;
        }

        List<Type> dummyDependencies = constructorParametersTypes.GetTypesExcluding(dependencies);

        foreach (Type dependency in dummyDependencies)
        {
            int index = constructorParametersTypes.GetIndexOf(dependency);

            if (dependency.IsInterface)
            {
                dependencies[index] = proxyGenerator.CreateInterfaceProxyWithoutTarget(dependency);
            }
            else
            {
                dependencies[index] = proxyGenerator.CreateClassProxy(dependency);
            }
        }

        object instance = Activator.CreateInstance(classType, dependencies)
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