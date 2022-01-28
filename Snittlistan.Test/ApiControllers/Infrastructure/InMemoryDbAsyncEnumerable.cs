using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

#nullable enable

namespace Snittlistan.Test.ApiControllers.Infrastructure;
public class InMemoryDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
{
    public InMemoryDbAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public InMemoryDbAsyncEnumerable(Expression expression)
        : base(expression)
    { }

    public IQueryProvider Provider => new InMemoryDbAsyncQueryProvider<T>(this);

    public IDbAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return new InMemoryDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
    {
        return GetAsyncEnumerator();
    }
}
