using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StatusCakeApi.Models
{
	public class UptimeTestResult
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("paused")]
		public bool Paused { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("website_url")]
		public string WebsiteUrl { get; set; }

		[JsonPropertyName("test_type")]
		public string TestType { get; set; }

		[JsonPropertyName("check_rate")]
		public long CheckRate { get; set; }

		
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("tags")]
		public List<string> Tags { get; set; }

		[JsonPropertyName("uptime")]
		public double Uptime { get; set; }
	}
}
