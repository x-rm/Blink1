using StatusCakeApi;
using System;
using System.Collections.Generic;
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

			int numberOfFailedTests = CheckAllStatusCakeTestsOK();

			if (numberOfFailedTests == 0)
			{
				// OK
				_blink1.SetColor(0, 30, 0);
				Console.WriteLine("No failed tests");
				
				// If there are no error conditions, then check SSL expirations:
				var numberOfSslFails = CheckSSLExpirations(_settings.CertificateExpirationDays);
				
				Console.WriteLine("SSL fails: " + numberOfSslFails);
				if (numberOfSslFails > 0)
				{
					_blink1.SetColor(60, 30, 0);
				}
			}
			else
			{
				Console.WriteLine("Failed tests: " + numberOfFailedTests);
				// NOT ok
				int brightness = numberOfFailedTests * 50;
				if (brightness >= MAX_BRIGHTNESS) brightness = MAX_BRIGHTNESS;

				_blink1.SetColor((ushort)(50 * brightness), 0, 0);
			}

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
	}
}
