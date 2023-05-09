using System;
using System.Collections.Generic;
using System.Text;

namespace XRMStatus
{
	public class Appointment
	{

		public Appointment(DateTime startDateTime)
		{
			StartDateTime = startDateTime;
		}

		public Appointment(string calendarName, string subject, DateTime startDateTime, DateTime endDateTime)
		{
			CalendarName = calendarName;
			Subject = subject;
			StartDateTime = startDateTime;
			EndDateTime = endDateTime;
		}

		public string CalendarName { get; set; }
		public string Subject { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
	}
}
