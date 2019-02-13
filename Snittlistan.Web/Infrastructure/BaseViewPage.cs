namespace Snittlistan.Web.Infrastructure
{
    using System.Web.Mvc;

    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual CustomPrincipal User => base.User as CustomPrincipal;
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual CustomPrincipal User => base.User as CustomPrincipal;
    }
}