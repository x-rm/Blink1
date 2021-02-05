using System;
using System.Collections.Generic;
using System.Text;

namespace XRMStatus
{
	public class StatusMonitorSettings
	{
		public string ApiUsername { get; set; }
		public string ApiPassword { get; set; }

		public int CertificateExpirationDays { get; set; }
	}
}
