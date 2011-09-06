using System.Web;

namespace Snittlistan.Infrastructure
{
	public class MailHttpResponse : HttpResponseBase
	{
		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}
	}
}