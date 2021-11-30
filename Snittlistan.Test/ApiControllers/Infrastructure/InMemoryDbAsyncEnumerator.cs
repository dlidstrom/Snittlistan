using System.Data.Entity.Infrastructure;

#nullable enable

namespace Snittlistan.Test.ApiControllers.Infrastructure;
public class InMemoryDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public InMemoryDbAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    object? IDbAsyncEnumerator.Current => Current;

    public void Dispose()
    {
        _inner.Dispose();
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_inner.MoveNext());
    }
}
