using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Reflection;
using Snittlistan.Web.Infrastructure.Database;

#nullable enable

namespace Snittlistan.Test.ApiControllers.Infrastructure;
public sealed class InMemoryDbSet<T> : IDbSet<T>, IDbAsyncEnumerable<T> where T : class
{
    private readonly IdGenerator _generator;
    private readonly HashSet<T> _data;
    private readonly IQueryable<T> _query;

    public InMemoryDbSet(IdGenerator generator)
    {
        _generator = generator;
        _data = new HashSet<T>();
        _query = _data.AsQueryable();
    }

    public ObservableCollection<T> Local => new(_data);

    public Type ElementType => _query.ElementType;

    public Expression Expression => _query.Expression;

    public IQueryProvider Provider => new InMemoryDbAsyncQueryProvider<T>(_query.Provider);

    public IQueryable<T> AsQueryable()
    {
        return _query;
    }

    public T Add(T entity)
    {
        _ = _data.Add(entity);
        Type type = typeof(T);
        string idPropertyName = $"{type.Name}Id";
        PropertyInfo propertyInfo = type.GetProperty(idPropertyName);
        if (propertyInfo == null)
        {
            throw new Exception($"No {idPropertyName} property found on {type.Name} class");
        }

        propertyInfo.SetValue(entity, _generator.GetNext());

        return entity;
    }

    public T Attach(T entity)
    {
        _ = _data.Add(entity);
        return entity;
    }

    public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
    {
        throw new NotImplementedException();
    }

    public T Create()
    {
        return Activator.CreateInstance<T>();
    }

    public T Find(params object[] keyValues)
    {
        throw new NotImplementedException("Derive from FakeDbSet and override Find");
    }

    public T Remove(T entity)
    {
        _ = _data.Remove(entity);
        return entity;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    public IDbAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return new InMemoryDbAsyncEnumerator<T>(GetEnumerator());
    }

    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
    {
        return GetAsyncEnumerator();
    }
}

public class InMemoryContext : ISnittlistanContext, IBitsContext
{
    public InMemoryContext()
    {
        IdGenerator generator = new();
        PublishedTasks = new InMemoryDbSet<PublishedTask>(generator);
        Tenants = new InMemoryDbSet<Tenant>(generator);
        Teams = new InMemoryDbSet<Bits_Team>(generator);
        Hallar = new InMemoryDbSet<Bits_Hall>(generator);
        RosterMails = new InMemoryDbSet<RosterMail>(generator);
        ChangeLogs = new InMemoryDbSet<ChangeLog>(generator);
        RateLimits = new InMemoryDbSet<RateLimit>(generator);
    }

    public IDbSet<PublishedTask> PublishedTasks { get; }

    public IDbSet<Bits_Team> Teams { get; }

    public IDbSet<Bits_Hall> Hallar { get; }

    public IDbSet<Tenant> Tenants { get; }

    public IDbSet<RosterMail> RosterMails { get; }

    public IDbSet<ChangeLog> ChangeLogs { get; }

    public IDbSet<RateLimit> RateLimits { get; }

    public DbChangeTracker ChangeTracker => throw new NotImplementedException();

    public int SaveChanges()
    {
        return 0;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }
}
