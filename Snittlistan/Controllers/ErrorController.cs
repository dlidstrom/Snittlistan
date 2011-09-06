using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;
using SnittListan.Helpers;

namespace SnittListan.Controllers
{
	public class ErrorController : AbstractController
	{
		public void LogJavaScriptError(string message)
		{
			ErrorSignal
				.FromCurrentContext()
				.Raise(new JavaScriptException(message));
		}
	}
}