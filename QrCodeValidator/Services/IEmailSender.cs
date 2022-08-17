using System;
namespace FinApp.Services
{
	public interface IEmailSender
	{
		void SendEmail(string fromAddress, string toAddress, string subject, string message);

	}
}

