using Company.Codex.DAL.Models;
using System.Net;
using System.Net.Mail;
namespace Company.Codex.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			// Mail Server : 

			// SMTP -> protocol for tranfer any electronic mail 

			var client = new SmtpClient("smtp.gmail.com", 587); // the protocol name , port depends on whether it is encrepted or not TLS is not
			client.EnableSsl = true;

			client.Credentials = new NetworkCredential("solimanbenkhali74@gmail.com", "bfynikohcwpvzlzy"); // Here I Will Never Write My Own Password .. instead ask google to give me a pass for a specific task

			client.Send("solimanbenkhali74@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
