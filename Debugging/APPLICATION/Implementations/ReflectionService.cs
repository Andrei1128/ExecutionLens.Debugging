using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.APPLICATION.Helpers;
using Debugging.DOMAIN.Models;
using Moq;
using System.Reflection;

namespace Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(ClassMock classMock, ClassMock? parent = null)
    {
        Type classType = Type.GetType(classMock.Class);

        List<object> dependencies = [];

        foreach (var interaction in classMock.Interactions)
        {
            dependencies.Add(CreateInstance(interaction, classMock));
        }

        IEnumerable<Type> dummyDependencies = GetParametersExcluding(GetConstructorParameters(classType), dependencies);

        foreach (var dependency in dummyDependencies)
        {
            dependencies.Add(proxyGenerator.CreateClassProxy(dependency));
            //dependencies.Add(CreateDummyMockInstance(dependency));
        }

        object? instance = Activator.CreateInstance(classType, [.. dependencies]);

        if (parent is null)
        {
            return instance;
        }

        var interceptor = new InterceptorService(classMock.Setups);

        var interfaceType = GetConstructorParameters(Type.GetType(parent.Class)).FirstOrDefault(p => p.IsAssignableFrom(classType));

        return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, instance, interceptor);
    }
    private object CreateDummyMockInstance(Type parameter)
    {
        Type genericMockType = typeof(Mock<>).MakeGenericType(parameter);
        var mock = Activator.CreateInstance(genericMockType);
        return ((Mock)mock).Object;
    }
    public static object[] NormalizeInputs(MethodInfo methodInfo, object[] inputs)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = Convert.ChangeType(inputs[i], parameterInfos[i].ParameterType);
        }
        return inputs;
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
