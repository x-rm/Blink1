using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StatusCakeApi.Models
{
	public class SslTestResults
	{
		[JsonPropertyName("data")]
		public List<SslTest> Data { get; set; }

		[JsonPropertyName("metadata")]
		public Metadata Metadata { get; set; }
	}
}
