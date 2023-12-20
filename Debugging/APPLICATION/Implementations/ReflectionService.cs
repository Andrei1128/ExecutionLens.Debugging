using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Models;
using Moq;
using PostMortem.SHARED.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Implementations;

internal class ReflectionService : IReflectionService
{
    private readonly ProxyGenerator proxyGenerator = new();
    public object CreateInstance(MethodLog log)
    {
        Type classType = Type.GetType(log.Entry.Class) ?? ;

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
            var filteredConstructorParameters = from param in constructorParameters
                                                where !dependencies.Any(x => param.ParameterType.IsAssignableFrom(x.GetType()))
                                                select param;

            foreach (var parameter in filteredConstructorParameters)
            {
                Type parameterType = parameter.ParameterType;

                var classNameList = from _log in log.Interactions
                                    where parameterType.IsAssignableFrom(Type.GetType(_log.Entry.Class))
                                    select _log.Entry.Class;

                if (classNameList.Any())
                {
                    var mocksList = from _log in log.Interactions
                                    where _log.Entry.Class == classNameList.First()
                                    select new MockObject(_log.Entry.Method, _log.Exit.Output);

                    var mockInterceptor = new InterceptorService(mocksList);
                    object? proxiedMock = proxyGenerator.CreateInterfaceProxyWithoutTarget(parameterType, mockInterceptor);
                    dependencies.Add(proxiedMock);
                }
                else
                {
                    Type genericMockType = typeof(Mock<>).MakeGenericType(parameterType);
                    var mock = Activator.CreateInstance(genericMockType);
                    dependencies.Add(((Mock)mock).Object);
                }
            }
        }
        if (dependencies.Count != 0)
        {
            return Activator.CreateInstance(classType, dependencies.ToArray());
        }
        return Activator.CreateInstance(classType);
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
