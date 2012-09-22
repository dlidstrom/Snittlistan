namespace Snittlistan.Web.Infrastructure
{
    using System;

    public class JavaScriptException : Exception
    {
        public JavaScriptException(string message)
            : base(message)
        { }
    }
}