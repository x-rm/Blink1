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
				ApiUsername = ConfigurationManager.AppSettings["ApiUsername"].ToString(),
				ApiPassword = ConfigurationManager.AppSettings["ApiPassword"].ToString(),
				CertificateExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["CertificateExpirationDays"])
			};

			Log($"START. Username: {settings.ApiUsername}");
			
			_statusMonitor = new StatusMonitor(settings);

			_timer = new Timer(0.3 * 60 * 1000); // every 1 minute
			_timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
			_timer.Start(); // <- important

			timer_Elapsed(null, null);
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				_statusMonitor.RunChecks();
				Log("OK");
			}
			catch (Exception ex)
			{
				Log(ex.ToString());
			}
		}

		protected override void OnStop()
		{
			//_statusMonitor.Close();
			Log("STOP");
		}

		public void Log(string Message)
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";

			using (StreamWriter sw = File.AppendText(filepath))
			{
				sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + Message);
			}
			
		}
	}
}
