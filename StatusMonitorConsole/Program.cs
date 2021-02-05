using System;
using XRMStatus;
using System.Configuration;

namespace StatusMonitorConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var settings = new StatusMonitorSettings
			{
				ApiUsername = ConfigurationManager.AppSettings["ApiUsername"].ToString(),
				ApiPassword = ConfigurationManager.AppSettings["ApiPassword"].ToString(),
				CertificateExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["CertificateExpirationDays"])
			};
			
			var statusMonitor = new StatusMonitor(settings);
			statusMonitor.RunChecks();
			
			Console.WriteLine("Press a key to exit");
			Console.ReadKey();
		}

	}
}
