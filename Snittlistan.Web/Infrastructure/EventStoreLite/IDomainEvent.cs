using System;

// ReSharper disable once CheckNamespace
namespace EventStoreLite
{
    /// <summary>
    /// Represents a domain event.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the event time stamp.
        /// </summary>
        DateTimeOffset TimeStamp { get; }

        /// <summary>
        /// Gets the change sequence.
        /// </summary>
        int ChangeSequence { get; }
    }
}
