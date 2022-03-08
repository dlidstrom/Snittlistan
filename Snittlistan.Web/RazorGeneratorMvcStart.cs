#nullable enable

using RazorGenerator.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(Snittlistan.Web.RazorGeneratorMvcStart),
    "Start")]

namespace Snittlistan.Web;

public static class RazorGeneratorMvcStart
{
    public static void Start()
    {
        PrecompiledMvcEngine engine = new(typeof(RazorGeneratorMvcStart).Assembly)
        {
            UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
        };

        ViewEngines.Engines.Insert(0, engine);

        // StartPage lookups are done by WebPages.
        VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
    }
}
