using System;

namespace Snittlistan.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IdAttribute : Attribute
    {
    }
}