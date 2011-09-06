using System.Web;

namespace SnittListan.Infrastructure
{
	public class MailHttpResponse : HttpResponseBase
	{
		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}
	}
}