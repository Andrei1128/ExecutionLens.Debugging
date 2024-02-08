namespace PostMortem.Logging.DOMAIN.Extensions;

internal static class TypeExtensions
{
    public static string GetClassName(this Type type) => 
        string.Join(",", type.AssemblyQualifiedName!.Split(',').Take(2));
}
