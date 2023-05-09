using System;
using System.Text.Json.Serialization;

namespace StatusCakeApi.Models
{
	public class SslTest
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("website_url")]
		public string WebsiteUrl { get; set; }
		
		[JsonPropertyName("valid_until")]
		public DateTimeOffset ValidUntil { get; set; }
	}
}
