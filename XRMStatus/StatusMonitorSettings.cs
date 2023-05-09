using System;
using System.Collections.Generic;
using System.Text;

namespace XRMStatus
{
	public class StatusMonitorSettings
	{
		public string StatusCakeApiKey { get; set; }

		public int CertificateExpirationDays { get; set; }

		public bool EnableCalendarFunction { get; set;}

		public string EmailAddress { get; set;}

    }
}
