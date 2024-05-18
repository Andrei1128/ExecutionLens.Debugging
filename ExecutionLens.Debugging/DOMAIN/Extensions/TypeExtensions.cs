using System.Reflection;

namespace ExecutionLens.Debugging.DOMAIN.Extensions;

internal static class TypeExtensions
{
    public static List<Type> GetConstructorParametersTypes(this Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();

        if (constructors.Length != 0)
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters()
                              .Select(x => x.ParameterType)
                              .ToList();
        }

        return [];
    }

    public static List<Type> GetTypesExcluding(this List<Type> types, object[] excluding)
    {
        IEnumerable<Type> excludingTypes =
            from type in types
            where !excluding.Any(x => type.IsAssignableFrom(x?.GetType()))
            select type;

        return excludingTypes.ToList() ?? [];
    }

    public static int GetIndexOf(this List<Type> types, Type type)
    {
        return types.FindIndex(t => t == type || type.IsAssignableTo(t));
    }
}
