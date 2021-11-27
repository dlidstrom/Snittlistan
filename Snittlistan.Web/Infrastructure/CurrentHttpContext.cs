#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Web;

    public static class CurrentHttpContext
    {
        public static Func<HttpContextBase> Instance = () => new HttpContextWrapper(HttpContext.Current);
    }
}
