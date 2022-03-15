#nullable enable

using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Reflection;
using Snittlistan.Web.Infrastructure.Database;

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
        Team = new InMemoryDbSet<Bits_Team>(generator);
        Hall = new InMemoryDbSet<Bits_Hall>(generator);
        RosterMails = new InMemoryDbSet<RosterMail>(generator);
        ChangeLogs = new InMemoryDbSet<ChangeLog>(generator);
        KeyValueProperties = new InMemoryDbSet<KeyValueProperty>(generator);
        SentEmails = new InMemoryDbSet<SentEmail>(generator);
        Match = new InMemoryDbSet<Bits_Match>(generator);
        TeamRef = new InMemoryDbSet<Bits_TeamRef>(generator);
        OilProfile = new InMemoryDbSet<Bits_OilProfile>(generator);
    }

    public IDbSet<PublishedTask> PublishedTasks { get; }

    public IDbSet<Bits_Team> Team { get; }

    public IDbSet<Bits_Hall> Hall { get; }

    public IDbSet<Bits_Match> Match { get; }

    public IDbSet<Bits_TeamRef> TeamRef { get; }

    public IDbSet<Bits_OilProfile> OilProfile { get; }

    public IDbSet<Tenant> Tenants { get; }

    public IDbSet<RosterMail> RosterMails { get; }

    public IDbSet<ChangeLog> ChangeLogs { get; }

    public IDbSet<KeyValueProperty> KeyValueProperties { get; }

    public IDbSet<SentEmail> SentEmails { get; }

    public DbChangeTracker ChangeTracker { get; } = null!;

    public int SaveChanges()
    {
        return 0;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }
}
