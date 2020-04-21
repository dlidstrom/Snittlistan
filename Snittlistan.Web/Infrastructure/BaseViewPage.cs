namespace Snittlistan.Web.Infrastructure
{
    using System.Web.Mvc;
    using Models;

    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual CustomPrincipal User => base.User as CustomPrincipal;

        public TenantConfiguration TenantConfiguration => DependencyResolver.Current.GetService<TenantConfiguration>();
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual CustomPrincipal User => base.User as CustomPrincipal;

        public TenantConfiguration TenantConfiguration => DependencyResolver.Current.GetService<TenantConfiguration>();
    }
}