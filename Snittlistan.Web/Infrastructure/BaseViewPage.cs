#nullable enable

using Snittlistan.Web.Infrastructure.Database;
using System.Web.Mvc;

namespace Snittlistan.Web.Infrastructure;

public abstract class BaseViewPage : WebViewPage
{
    public new virtual CustomPrincipal User => (CustomPrincipal)base.User;

    public Tenant Tenant => DependencyResolver.Current.GetService<Tenant>();
}

public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
{
    public new virtual CustomPrincipal User => (CustomPrincipal)base.User;

    public Tenant Tenant => DependencyResolver.Current.GetService<Tenant>();
}
