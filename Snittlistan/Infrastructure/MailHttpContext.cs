using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace Snittlistan.Infrastructure
{
	public class MailHttpContext : HttpContextBase
	{
		private readonly IDictionary items = new Hashtable();

		public override IDictionary Items
		{
			get { return items; }
		}

		public override Cache Cache
		{
			get { return HttpRuntime.Cache; }
		}

		public override HttpResponseBase Response
		{
			get
			{
				return new MailHttpResponse();
			}
		}

		public override HttpRequestBase Request
		{
			get
			{
				return new HttpRequestWrapper(
					new HttpRequest(string.Empty, ConfigurationManager.AppSettings["MainUrl"], string.Empty));
			}
		}
	}
}