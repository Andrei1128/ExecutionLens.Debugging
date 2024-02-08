﻿using System.Reflection;

namespace PostMortem.Debugging.DOMAIN.Extensions;

public static class MethodInfoExtensions
{
    public static object[] NormalizeParametersType(this MethodInfo methodInfo, params object[] parameters)
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