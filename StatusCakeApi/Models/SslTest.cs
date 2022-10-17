using System;
using Newtonsoft.Json;

namespace StatusCakeApi.Models
{
	public class SslTest
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("website_url")]
		public string WebsiteUrl { get; set; }
		
		[JsonProperty("valid_until")]
		public DateTimeOffset ValidUntil { get; set; }
	}
}
