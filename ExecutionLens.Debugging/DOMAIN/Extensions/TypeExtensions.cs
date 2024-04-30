using System.Reflection;

namespace ExecutionLens.Debugging.DOMAIN.Extensions;

internal static class TypeExtensions
{
    public static IEnumerable<Type> GetConstructorParametersTypes(this Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();

        if (constructors.Length != 0)
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters().Select(x => x.ParameterType);
        }

        return [];
    }

    public static IEnumerable<Type> GetTypesExcluding(this IEnumerable<Type> types, object[] excluding)
    {
        IEnumerable<Type> excludingTypes =
            from type in types
            where !excluding.Any(x => type.IsAssignableFrom(x.GetType()))
            select type;

        return excludingTypes ?? [];
    }
}
