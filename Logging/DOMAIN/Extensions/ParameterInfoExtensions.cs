using System.Reflection;

namespace Logging.DOMAIN.Extensions;

internal static class ParameterInfoExtensions
{
    public static string[] GetTypesName(this ParameterInfo[] parametersInfo)
    {
        var names = new List<string>();

        foreach (var p in parametersInfo)
        {
            string? name = p.ParameterType.Name;

            if (name is not null)
            {
                names.Add(name);
            }
        }

        return [.. names];
    }
}
