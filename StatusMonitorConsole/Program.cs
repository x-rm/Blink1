using System;
using XRMStatus;
using System.Configuration;
using System.Threading.Tasks;
using NLog;
using System.Threading;

namespace StatusMonitorConsole
{
	class Program
	{
		private static Logger _logger = LogManager.GetLogger("StatusMonitorConsole");

		static async Task Main(string[] args)
		{
			var settings = new StatusMonitorSettings
			{
				StatusCakeApiKey = ConfigurationManager.AppSettings["StatusCakeApiKey"].ToString(),
				CertificateExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["CertificateExpirationDays"]),
                EnableCalendarFunction = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCalendarFunction"]),
				EmailAddress = ConfigurationManager.AppSettings["EmailAddress"]
            };


			var statusMonitor = new StatusMonitor(settings, _logger);

            while (!Console.KeyAvailable)
			{
				await statusMonitor.RunChecksAsync();
				Console.WriteLine("Running again in 5 seconds. Press a key to exit...");
				Thread.Sleep(5000);
			}

			Console.WriteLine("Press a key to exit");
			Console.ReadKey();
		}

	}
}
