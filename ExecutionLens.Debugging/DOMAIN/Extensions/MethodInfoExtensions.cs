using ExecutionLens.Debugging.DOMAIN.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace ExecutionLens.Debugging.DOMAIN.Extensions;

internal static class MethodInfoExtensions
{
    public static object[] NormalizeParametersType(this MethodInfo methodInfo, Property[] parameters)
    {
        object[] normalizedParameters = new object[parameters.Length];

        ParameterInfo[] parameterInfos = methodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            normalizedParameters[i] = parameters[i].Type == "String" 
                                    ? parameters[i].Value 
                                    : JsonConvert.DeserializeObject(parameters[i].Value, parameterInfos[i].ParameterType);
        }

        return normalizedParameters;
    }
}