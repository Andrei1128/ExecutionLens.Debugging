using Microsoft.AspNetCore.Mvc.Abstractions;

namespace PostMortem.Logging.DOMAIN.Extensions;

internal static class ActionDescriptorExtensions
{
    public static string GetMethodName(this ActionDescriptor actionDescriptor)
    {
        string fullName = actionDescriptor.DisplayName;

        int startIndex = fullName.LastIndexOf('.') + 1;
        int endIndex = fullName.IndexOf(' ', startIndex);

        return fullName[startIndex..endIndex];
    }
}
