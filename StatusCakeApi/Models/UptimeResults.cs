using System.Collections.Generic;

namespace StatusCakeApi.Models
{
	public class UptimeResults
	{

		public List<UptimeTestResult> Data { get; set; }
		public Metadata Metadata { get; set; }
	}
}
