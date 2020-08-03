// ReSharper disable UseNameofExpression
namespace Snittlistan.Queue
{
    using System.Diagnostics;
    using System.Threading;

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
            Interlocked.Increment(ref _value);
        }

        public void Decrement()
        {
            Interlocked.Decrement(ref _value);
        }
    }
}