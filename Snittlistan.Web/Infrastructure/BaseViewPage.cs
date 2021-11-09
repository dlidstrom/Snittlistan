#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System.Web.Mvc;
    using Snittlistan.Queue.Models;

    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual CustomPrincipal User => (CustomPrincipal)base.User;

        public TenantConfiguration TenantConfiguration => DependencyResolver.Current.GetService<TenantConfiguration>();
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual CustomPrincipal User => (CustomPrincipal)base.User;

        public TenantConfiguration TenantConfiguration => DependencyResolver.Current.GetService<TenantConfiguration>();
    }
}
