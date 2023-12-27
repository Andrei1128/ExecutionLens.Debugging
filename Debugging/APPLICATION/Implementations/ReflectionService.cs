using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.APPLICATION.Helpers;
using Debugging.DOMAIN.Utilities;
using Moq;
using PostMortem.SHARED.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(MethodLog log)
    {
        Type classType = GetType(log.Entry.Class);

        List<object> dependencies = [];

        //creaza instante pentru depenndintelete aflate in interactiuni
        foreach (var dep in log.Interactions)
        {
            dependencies.Add(CreateInstance(dep));
        }

        ParameterInfo[] constructorParameters = GetConstructorParameters(classType);

        if (constructorParameters.Length != 0)
        {
            //filtreaza parametrii constructorului care nu sunt in interactiuni, deci nu sunt deja in dependencies
            var filteredTypes = constructorParameters.GetParametersExcluding(dependencies);

            foreach (var parameter in filteredTypes)
            {
                var interactionsNameList = GetInteractionsNames(log.Interactions, parameter);

                if (interactionsNameList.Any()) //creaza instanta pentru dependinta care este interactiune
                {
                    var mocksList = (from _log in log.Interactions
                                     where _log.Entry.Class == interactionsNameList.First()
                                     select MethodMockFactory.Create(_log.Entry.Method, _log.Entry.Input, _log.Exit.Output))
                                    .ToList();

                    var mockInterceptor = new InterceptorService(mocksList);
                    object? proxiedMock = proxyGenerator.CreateInterfaceProxyWithoutTarget(parameter, mockInterceptor);
                    dependencies.Add(proxiedMock);
                }
                else //create mock dummy pentru restu dependintelor care nu sunt folosite..
                {
                    dependencies.Add(CreateDummyMockInstance(parameter));
                }
            }
        }
        if (dependencies.Count != 0)
        {
            return Activator.CreateInstance(classType, dependencies.ToArray());
        }
        return Activator.CreateInstance(classType);
    }

    private object CreateDummyMockInstance(Type parameter)
    {
        Type genericMockType = typeof(Mock<>).MakeGenericType(parameter);
        var mock = Activator.CreateInstance(genericMockType);
        return ((Mock)mock).Object;
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

    private ParameterInfo[] GetConstructorParameters(Type classType)
    {
        ConstructorInfo[] constructors = classType.GetConstructors();

        if (constructors.Length != 0)
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters();
        }

        return [];
    }
}
