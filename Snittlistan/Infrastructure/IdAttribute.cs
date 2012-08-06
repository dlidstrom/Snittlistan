namespace Snittlistan.Infrastructure
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IdAttribute : Attribute
    {
    }
}