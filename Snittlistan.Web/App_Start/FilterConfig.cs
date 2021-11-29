#nullable enable

namespace Snittlistan.Web
{
    using System.Web.Mvc;
    using Snittlistan.Web.Infrastructure.Attributes;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CorrelationIdFilterAttribute());
            filters.Add(new ValidateModelAttributeMvc());
        }
    }
}
