using StatusCakeApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using ThingM.Blink1;

namespace XRMStatus
{
	public class StatusMonitor
	{
		private Blink1 _blink1;
		private readonly StatusCakeApiClient _api;
		private readonly StatusMonitorSettings _settings;
		const int MAX_BRIGHTNESS = 200;
		private Color lastColor;

		public StatusMonitor(StatusMonitorSettings settings)
		{
			_settings = settings;
			_api = new StatusCakeApiClient(settings.ApiUsername, settings.ApiPassword);
		}

		//public void StartupLights()
		//{
		//	blink1 = new Blink1();
		
		//	blink1.Open();

		//	blink1.SetColor(150, 0, 0); // Red
		//	blink1.FadeToColor(500, 0, 150, 0, true); // green
		//	blink1.FadeToColor(500, 0, 0, 150, true); // blue
		//	blink1.FadeToColor(500, 150, 0, 0, true); // red
		//	blink1.FadeToColor(500, 0, 150, 0, true); // green
		//	blink1.FadeToColor(500, 0, 0, 150, true); // blue		
		//	blink1.SetColor(0, 30, 0);

		//	blink1.Close(false);
		//}


		public void RunChecks()
		{
			_blink1 = new Blink1();
			_blink1.Open();
			Color color = new Color();
				
			int numberOfFailedTests = CheckAllStatusCakeTestsOK();

			if (numberOfFailedTests == 0)
			{
				// OK
				color = Color.FromArgb(0, 50, 0);
				Log("No failed tests");
				
				// If there are no error conditions, then check SSL expirations:
				var numberOfSslFails = CheckSSLExpirations(_settings.CertificateExpirationDays);
				
				Log("SSL fails: " + numberOfSslFails);
				if (numberOfSslFails > 0)
				{
					color = ColorHelper.ChangeBrightness(Color.DarkOrange, -0.7F);
				}
			}
			else
			{
				// NOT ok
				Log("Test fails: " + numberOfFailedTests);
				
				int brightness = numberOfFailedTests * 50;
				if (brightness >= MAX_BRIGHTNESS) brightness = MAX_BRIGHTNESS;

				color = Color.FromArgb(brightness, 0, 0);
			}

			// Don't update LED unless color has changed.
			if (lastColor != color)
			{
				Log($"Changing LED color to: {color.R},{color.G},{color.B}");
			}
			
			_blink1.SetColor(color.R, color.G, color.B);
			
			_blink1.Close(false);
			_blink1 = null;
		}

		public void Close()
		{
			_blink1.Close();
		}


		private int CheckSSLExpirations(int days)
		{
			List<SslDto> tests = _api.GetSSLTests();
			
			int numberOfFailedTests = 0;

			foreach (var test in tests)
			{
				if (test.ValidUntilUtc <= DateTime.UtcNow.AddDays(days)) numberOfFailedTests++;
			}

			return numberOfFailedTests;
		}
		
		private int CheckAllStatusCakeTestsOK()
		{
			
			List<TestDto> tests = _api.GetTests();

			int numberOfFailedTests = 0;

			foreach (var test in tests)
			{
				if (test.Status.ToLower() != "up") numberOfFailedTests++;
			}

			return numberOfFailedTests;
		}
		
		public void Log(string message)
		{
			Console.WriteLine(message);
			
			string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";

			using (StreamWriter sw = File.AppendText(filepath))
			{
				sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + message);
			}
			
		}
	}
}
