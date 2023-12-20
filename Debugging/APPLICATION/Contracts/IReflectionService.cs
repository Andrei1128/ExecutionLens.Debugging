using PostMortem.SHARED.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Contracts;

public interface IReflectionService
{
    object? CreateInstance(MethodLog log);
    void NormalizeInputs(MethodInfo methodInfo, object[] inputs);
    Type GetType(string typeName);
}
