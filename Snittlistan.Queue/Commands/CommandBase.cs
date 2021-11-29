#nullable enable

namespace Snittlistan.Queue.Commands
{
    using System;

    public abstract class CommandBase
    {
        public CommandBase()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; }
    }
}
