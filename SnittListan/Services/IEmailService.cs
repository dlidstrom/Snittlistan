using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnittListan.Services
{
	public interface IEmailService
	{
		void SendMail(string recipient, string subject, string body);
	}
}
