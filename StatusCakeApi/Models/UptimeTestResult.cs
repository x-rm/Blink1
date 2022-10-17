using System.Collections.Generic;
using Newtonsoft.Json;

namespace StatusCakeApi.Models
{
	public class UptimeTestResult
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("paused")]
		public bool Paused { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("website_url")]
		public string WebsiteUrl { get; set; }

		[JsonProperty("test_type")]
		public string TestType { get; set; }

		[JsonProperty("check_rate")]
		public long CheckRate { get; set; }

		
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("tags")]
		public List<string> Tags { get; set; }

		[JsonProperty("uptime")]
		public double Uptime { get; set; }
	}
}
