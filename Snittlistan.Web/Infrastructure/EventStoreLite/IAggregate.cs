 // ReSharper disable once CheckNamespace
namespace EventStoreLite
{
    internal interface IAggregate
    {
        string Id { get; }
        IDomainEvent[] GetUncommittedChanges();
    }
}
