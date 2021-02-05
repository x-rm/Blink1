using System;
using Newtonsoft.Json;

namespace StatusCakeApi
{
	public class SslDto
	{
		public int Id { get; set; }

		public string Domain { get; set; }
		
		[JsonProperty("valid_until_utc")]
		public DateTime ValidUntilUtc { get; set; }
	}
}
