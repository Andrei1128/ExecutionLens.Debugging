using PostMortem.Debugging.DOMAIN.Models;
using System.Reflection;

namespace PostMortem.Debugging.APPLICATION.Contracts;

internal interface IReflectionService
{
    object CreateInstance(Mock mock, Mock? parent = null);
    object[] NormalizeParametersType(MethodInfo methodInfo, params object[] parameters);
}
