using System.Web;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public static class CurrentHttpContext
{
    public static Func<HttpContextBase> Instance = () => new HttpContextWrapper(HttpContext.Current);
}
