using System;

namespace Snittlistan.Queue.Messages
{
    public record BusinessKey(Type MessageType, string Key);
}
