// ReSharper disable once CheckNamespace
namespace EventStoreLite.Infrastructure
{
    using System.Diagnostics;

    internal static class PrivateReflectionDynamicObjectExtensions
    {
        [DebuggerStepThrough]
        public static dynamic AsDynamic(this object o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
}