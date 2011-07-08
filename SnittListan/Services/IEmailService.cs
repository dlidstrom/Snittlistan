namespace SnittListan.Services
{
	public interface IEmailService
	{
		void SendMail(string recipient, string subject, string body);
	}
}