using System.Reflection;

namespace Debugging.APPLICATION.Helpers;

internal static class ExtensionMethods
{
    public static IEnumerable<Type>? GetParametersExcluding(this ParameterInfo[] parameters, List<object> excludeList)
    {
        return from param in parameters
               where !excludeList.Any(x => param.ParameterType.IsAssignableFrom(x.GetType()))
               select param.ParameterType;
    }
}
