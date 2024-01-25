using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.APPLICATION.Helpers;
using Debugging.DOMAIN.Models;
using Moq;
using PostMortem.SHARED.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(ClassMock classMock)
    {
        Type classType = GetType(classMock.Class);

        List<object> dependencies = [];

        foreach (var interaction in classMock.Interactions)
        {
            dependencies.Add(CreateInstance(interaction));
        }

        var dummyDependencies = GetConstructorParameters(classType).GetParametersExcluding(dependencies);

        foreach (var dependency in dummyDependencies)
        {
            dependencies.Add(CreateDummyMockInstance(dependency));
        }

        var interceptor = new InterceptorService(classMock.Setups);
        return proxyGenerator.CreateClassProxy(classType, [.. dependencies], interceptor);
    }
    #region METHODS
    private object CreateDummyMockInstance(Type parameter)
    {
        Type genericMockType = typeof(Mock<>).MakeGenericType(parameter);
        var mock = Activator.CreateInstance(genericMockType);
        return ((Mock)mock).Object;
    }

    private bool IsInConstructorParameters(string className, Type[] parameters)
    {
        return parameters.Any(p => p.IsAssignableFrom(Type.GetType(className)));
    }

    private IEnumerable<string> GetInteractionsNames(List<MethodLog> interactions, Type parameter)
    {
        return from _log in interactions
               where parameter.IsAssignableFrom(Type.GetType(_log.Entry.Class))
               select _log.Entry.Class;
    }
    public void NormalizeInputs(MethodInfo methodInfo, object[] inputs)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = Convert.ChangeType(inputs[i], parameterInfos[i].ParameterType);
        }
    }

    public Type GetType(string typeName) => Type.GetType(typeName) ?? throw new Exception($"Could not find type {typeName}!");

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
    #endregion
}
