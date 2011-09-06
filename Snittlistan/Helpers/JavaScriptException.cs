using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snittlistan.Helpers
{
	public class JavaScriptException : Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{ }
	}
}