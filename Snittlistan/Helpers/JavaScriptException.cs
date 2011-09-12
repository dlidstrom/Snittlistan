using System;

namespace Snittlistan.Helpers
{
	public class JavaScriptException : Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{ }
	}
}