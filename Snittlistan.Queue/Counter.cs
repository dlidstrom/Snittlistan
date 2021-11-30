
using System.Diagnostics;

#nullable enable

namespace Snittlistan.Queue;
/// <summary>
/// Implements a thread-safe counter.
/// </summary>
[DebuggerDisplay("Value = {Value}")]
public class Counter
{
    private int _value;

    public int Value => _value;

    public void Increment()
    {
        _ = Interlocked.Increment(ref _value);
    }

    public void Decrement()
    {
        _ = Interlocked.Decrement(ref _value);
    }
}
