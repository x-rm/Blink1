using System.Collections.Generic;
using Newtonsoft.Json;

namespace StatusCakeApi.Models
{
	public class SslTestResults
	{
		[JsonProperty("data")]
		public List<SslTest> Data { get; set; }

		[JsonProperty("metadata")]
		public Metadata Metadata { get; set; }
	}
}
