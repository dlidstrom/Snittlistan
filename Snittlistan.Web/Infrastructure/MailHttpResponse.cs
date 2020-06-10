namespace Snittlistan.Web.Infrastructure
{
    using System.Web;

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
}