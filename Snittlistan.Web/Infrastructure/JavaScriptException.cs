namespace Snittlistan.Web.Infrastructure;
public class JavaScriptException : Exception
{
    public JavaScriptException(string message)
        : base(message)
    { }
}
