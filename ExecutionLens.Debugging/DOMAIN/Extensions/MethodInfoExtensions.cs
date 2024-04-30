using System.Reflection;

namespace ExecutionLens.Debugging.DOMAIN.Extensions;

internal static class MethodInfoExtensions
{
    public static object[] NormalizeParametersType(this MethodInfo methodInfo, object[] parameters)
    {
        object[] normalizedParameters = new object[parameters.Length];

        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            normalizedParameters[i] = Convert.ChangeType(parameters[i], parameterInfos[i].ParameterType);
        }

        return normalizedParameters;
    }
}
