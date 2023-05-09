using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XRMStatus
{
	public class CalendarService
	{
		readonly RestClient _client;

		public CalendarService(string baseUrl)
		{
			_client = new RestClient(baseUrl);
		}

		public async Task<List<Appointment>> GetAppointmentsAsync(string emailAddress)
		{
			List<Appointment> appointments = new List<Appointment>();

			RestRequest request = new RestRequest("", Method.Post);
			request.AddQueryParameter("view", "fbinfo");
			request.AddQueryParameter("user", "nick@x-rm.com");
			var response = await _client.ExecuteAsync(request);

			using (var reader = new StringReader(response.Content))
			{
				for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
				{
					if (line.StartsWith("FREEBUSY;")) 
					{
						DateTime? start = ParseFreeBusy(line);

						if (start != null && start >= DateTime.UtcNow.AddMinutes(-5) && start <= DateTime.UtcNow.AddDays(1))
						{
							var	app  = new Appointment(start.Value);
							appointments.Add(app);
						}
					}
				}
			}

			return appointments;
		}

		private DateTime? ParseFreeBusy(string line) 
		{
			var s = line.Replace("=",";").Replace(":",";").Replace("/", ";");
			var arr = s.Split(';');

			DateTime startDate;
			DateTime.TryParseExact(arr[3], "yyyyMMddTHHmmssZ", new CultureInfo("en-GB"), DateTimeStyles.AdjustToUniversal, out startDate);
			
			return startDate;
		}

	}
}
