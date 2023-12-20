using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.DOMAIN.Utilities;

internal class MethodLogFactory
{
    public static MethodLog Create(MethodEntry logEntry) => new MethodLog()
    {
        Id = 0,
        Entry = logEntry
    };
}
