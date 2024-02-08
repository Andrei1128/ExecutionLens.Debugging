using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodLogFactory
{
    public static MethodLog Create(MethodEntry logEntry) =>
        new()
        {
            Entry = logEntry
        };
}
