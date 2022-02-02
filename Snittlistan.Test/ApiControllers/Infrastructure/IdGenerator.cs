#nullable enable

namespace Snittlistan.Test.ApiControllers.Infrastructure;

public class IdGenerator
{
    private int _currentId;

    public int GetNext()
    {
        return ++_currentId;
    }
}
