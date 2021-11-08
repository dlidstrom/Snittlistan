#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;

    public record BusinessKey(Type MessageType, string Key);
}
