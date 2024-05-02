using Castle.DynamicProxy;

namespace ExecutionLens.Debugging.DOMAIN.Extensions;

internal static class ObjectListExtensions
{
    public static IEnumerable<object> SortAs(this List<object> source, IEnumerable<Type> targetOrder)
    {
        Dictionary<Type, int> orderMap = targetOrder.Select((type, index) => new { type, index })
                                                    .ToDictionary(t => t.type, t => t.index);

        return source.OrderBy(item =>
        {
            Type itemType = GetUnproxiedType(item);

            var index = itemType.GetInterfaces()
                                .Select(i => orderMap.TryGetValue(i, out int idx) ? idx : (int?)null)
                                .FirstOrDefault(i => i.HasValue) ?? int.MaxValue;
            return index;
        });
    }

    private static Type GetUnproxiedType(object obj)
    {
        return obj is IProxyTargetAccessor accessor
                        ? accessor.DynProxyGetTarget().GetType()
                        : obj.GetType();
    }
}
