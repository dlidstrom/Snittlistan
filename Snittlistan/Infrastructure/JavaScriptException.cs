namespace Snittlistan.Infrastructure
{
    using System;

    public class JavaScriptException : Exception
    {
        public JavaScriptException(string message)
            : base(message)
        { }
    }
}