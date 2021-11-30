
using System.Web.Mvc;
using Snittlistan.Web.Infrastructure.Attributes;

#nullable enable

namespace Snittlistan.Web;
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        filters.Add(new CorrelationIdFilterAttribute());
        filters.Add(new ValidateModelAttributeMvc());
    }
}
