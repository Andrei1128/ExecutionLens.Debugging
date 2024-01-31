using Debugging.DOMAIN.Models;

namespace Debugging.APPLICATION.Contracts;

public interface IReflectionService
{
    object? CreateInstance(ClassMock log, ClassMock? parent = null);
}
