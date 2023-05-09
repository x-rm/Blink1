using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using XRMStatus;

namespace XRMStatus
{
	public partial class WindowsService : ServiceBase
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		Timer _timer;
		StatusMonitor _statusMonitor;

		public WindowsService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			var settings = new StatusMonitorSettings
			{
				StatusCakeApiKey = ConfigurationManager.AppSettings["StatusCakeApiKey"].ToString(),
				CertificateExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["CertificateExpirationDays"]),
                EnableCalendarFunction = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCalendarFunction"]),
				EmailAddress = ConfigurationManager.AppSettings["EmailAddress"]
            };

			_logger.Info($"START. StatusCakeApiKey: {settings.StatusCakeApiKey}");
			
			try {
				_statusMonitor = new StatusMonitor(settings, _logger);

				_timer = new Timer(0.3 * 60 * 1000); // every 1 minute
				_timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
				_timer.Start(); // <- important

				timer_Elapsed(null, null);
			} 
			catch (Exception ex) {
				_logger.Fatal(ex, "Fatal error starting app");
			}
		}

		private async void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				await _statusMonitor.RunChecksAsync();
				_logger.Info("OK");
			}
			catch (Exception ex)
			{
				_logger.Fatal(ex.ToString());
			}
		}

		protected override void OnStop()
		{
			//_statusMonitor.Close();
			_logger.Info("STOP");
		}

	}
}
