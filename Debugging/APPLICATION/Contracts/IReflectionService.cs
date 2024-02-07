using Debugging.DOMAIN.Models;

namespace Debugging.APPLICATION.Contracts;

public interface IReflectionService
{
    object CreateInstance(Mock mock, Mock? parent = null);
}
