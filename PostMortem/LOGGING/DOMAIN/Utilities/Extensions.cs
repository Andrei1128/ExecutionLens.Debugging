using Microsoft.AspNetCore.Mvc.Abstractions;

namespace PostMortem.LOGGING.DOMAIN.Utilities;

public static class Extensions
{
    public static string GetClassName(this Type type) => string.Join(",", type.AssemblyQualifiedName!.Split(',').Take(2));
    public static string GetMethodName(this ActionDescriptor actionDescriptor)
    {
        string fullName = actionDescriptor.DisplayName;

        int startIndex = fullName.LastIndexOf('.') + 1;
        int endIndex = fullName.IndexOf(' ', startIndex);

        return fullName[startIndex..endIndex];
    }
}
