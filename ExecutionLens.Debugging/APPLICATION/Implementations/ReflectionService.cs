using Castle.DynamicProxy;
using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.DOMAIN.Extensions;
using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly IProxyGenerator _proxyGenerator = new ProxyGenerator();
    private readonly IInterceptor _nullInterceptor = new NullInterceptor();
    
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
                dependencies[index] = _proxyGenerator.CreateInterfaceProxyWithoutTarget(dependency, _nullInterceptor);
            }
            else
            {
                dependencies[index] = _proxyGenerator.CreateClassProxy(dependency, _nullInterceptor);
            }
        }

        object instance = Activator.CreateInstance(classType, dependencies)
            ?? throw new Exception($"Could not create instance for `{classType}`!");

        if (parent is null)
        {
            return instance;
        }

        IInterceptor interceptor = new ConsistencyInterceptor(mock.Setups);

        Type parentClassType = Type.GetType(parent.Class)
            ?? throw new Exception($"Type `{mock.Class}` not found!");

        Type interfaceType = parentClassType.GetConstructorParametersTypes()
                                            .First(p => p.IsAssignableFrom(classType));

        return _proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, instance, interceptor);
    }
}