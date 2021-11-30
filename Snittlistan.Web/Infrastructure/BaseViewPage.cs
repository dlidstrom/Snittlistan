#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System.Web.Mvc;

    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual CustomPrincipal User => (CustomPrincipal)base.User;
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual CustomPrincipal User => (CustomPrincipal)base.User;
    }
}
