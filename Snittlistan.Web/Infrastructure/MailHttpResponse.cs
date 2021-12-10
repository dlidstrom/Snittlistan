
using System.Web;

namespace Snittlistan.Web.Infrastructure;
public class MailHttpResponse : HttpResponseBase
{
    /// <summary>
    /// Is this really necessary.
    /// </summary>
    /// <param name="virtualPath"></param>
    /// <returns></returns>
    public override string ApplyAppPathModifier(string virtualPath)
    {
        return virtualPath;
    }
}
