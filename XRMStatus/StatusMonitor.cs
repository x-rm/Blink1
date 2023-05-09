using StatusCakeApi;
using System;
using System.Drawing;
using System.Threading.Tasks;
using StatusCakeApi.Models;
using ThingM.Blink1;
using System.Linq;
using NLog;

namespace XRMStatus
{
	public class StatusMonitor
	{
		private Blink1 _blink1;
		private readonly StatusCakeApiClient _api;
		private readonly StatusMonitorSettings _settings;
		private readonly Logger _logger;


		const int MAX_BRIGHTNESS = 200;
		private Color lastColor;

		public StatusMonitor(StatusMonitorSettings settings, Logger logger)
		{
			_settings = settings;
			_api = new StatusCakeApiClient(settings.StatusCakeApiKey, logger);
			_logger = logger;
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


		public async Task RunChecksAsync()
		{
			_blink1 = new Blink1();
			_blink1.Open();
			Color color = new Color();
				
			int numberOfFailedTests = await CheckAllStatusCakeTestsOKAsync();

			if (numberOfFailedTests == 0)
			{
				// OK
				color = Color.FromArgb(0, 50, 0);
				Log("No failed tests");
				
				// If there are no error conditions, then check SSL expirations:
				try
				{
					var numberOfSslFails = await CheckSSLExpirationsAsync(_settings.CertificateExpirationDays);

					Log("SSL fails: " + numberOfSslFails);
					if (numberOfSslFails > 0)
					{
						color = ColorHelper.ChangeBrightness(Color.DarkOrange, -0.6F);
					}
				}
				catch (Exception ex)
				{
					Log("Error: " + ex.ToString());
					color = ColorHelper.ChangeBrightness(Color.BlueViolet, -0.9F);
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

			bool noMeetingsSoon = true;



			if (_settings.EnableCalendarFunction) 
			{
				Log("Calendar function enabled");
				var calendar = new CalendarService("https://mail.x-rm.com/WorldClient.dll");
				var appointments = await calendar.GetAppointmentsAsync("nick@x-rm.com");
                var upcoming = appointments.Where(t => t.StartDateTime >= DateTime.UtcNow.AddMinutes(-2) && t.StartDateTime <= DateTime.UtcNow.AddMinutes(5)).ToList();

				if (upcoming.Count > 0) 
				{
                    noMeetingsSoon = false;

                    double secondsUntil = SecondsUntil(upcoming.First().StartDateTime);

                    Log($"Upcoming appointment in {secondsUntil} seconds");
					color = ColorHelper.ChangeBrightness(Color.Blue, -0.6F);
                    //_blink1.SetColor(color.R, color.G, color.B);

                    if (secondsUntil < 0) 
					{
						// if meeting has just started, blink RED:
                        color = ColorHelper.ChangeBrightness(Color.Red, -0.6F);
                        _blink1.Blink(31, 300, 300, color.R, color.G, color.B);
                    } 
					else if (secondsUntil <= 60) 
					{
                        // if meeting starts in less than 1 min, blink BLUE:
                        color = ColorHelper.ChangeBrightness(Color.Blue, -0.6F);
                        _blink1.Blink(31, 300, 300, color.R, color.G, color.B);
                        
					} 
					else if (secondsUntil <= 120) 
					{
                        color = ColorHelper.ChangeBrightness(Color.Blue, -0.3F);
                        //_blink1.Blink(30, 1000, 1000, color.R, color.G, color.B);
                    }

                    _blink1.SetColor(color.R, color.G, color.B);
				}
            }
			else
			{
				Log("Calendar function disabled");
			}

            // Don't update LED unless color has changed.
            if (lastColor != color)
            {
                Log($"Changing LED color to: {color.R},{color.G},{color.B}");
                _blink1.SetColor(color.R, color.G, color.B);
            }  

            _blink1.Close(false);
			_blink1 = null;
		}

		public void Close()
		{
			_blink1.Close();
		}

		private double SecondsUntil(DateTime start )
		{
			return (start - DateTime.UtcNow).TotalSeconds;
		}

		private async Task<int> CheckSSLExpirationsAsync(int days)
		{
			var results = await _api.GetSSLTestsAsync();

			if (results.Data.Count == 0) throw new ApplicationException("No SSL results were returned");

			int numberOfFailedTests = 0;

			foreach (var test in results.Data)
			{
				if (test.ValidUntil <= DateTime.UtcNow.AddDays(days)) numberOfFailedTests++;
			}

			return numberOfFailedTests;
		}
		
		private async Task<int> CheckAllStatusCakeTestsOKAsync()
		{
			UptimeResults failedTests = await _api.GetFailedTestsAsync();
			 
			return failedTests.Data.Count;
		}
		
		public void Log(string message)
		{
			_logger.Info(message);
		}
	}
}
