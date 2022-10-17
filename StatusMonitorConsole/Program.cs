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
				ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString(),
				CertificateExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["CertificateExpirationDays"])
			};
			
			var statusMonitor = new StatusMonitor(settings);
			statusMonitor.RunChecks();
			
			Console.WriteLine("Press a key to exit");
			Console.ReadKey();
		}

	}
}
