using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using StatusCakeApi.Models;

namespace StatusCakeApi
{

    public class StatusCakeApiClient	
    {
		const string BaseUrl = "https://api.statuscake.com/v1/";
		readonly IRestClient client;

		public StatusCakeApiClient(string apiKey)
		{
			client = new RestClient(BaseUrl);
			client.Authenticator = new JwtAuthenticator(apiKey);
			client.UseNewtonsoftJson();
		}

		public UptimeResults GetTests()
		{
			var request = new RestRequest("uptime");
			var tests = client.Get<UptimeResults>(request);
			return tests.Data;
		}

		public SslTestResults GetSSLTests()
		{
			var request = new RestRequest("ssl");
			var tests = client.Get<SslTestResults>(request);
			return tests.Data;
		}
		
		public T Execute<T>(RestRequest request) where T : new()
		{	
			var response = client.Execute<T>(request);

			if (response.ErrorException != null)
			{
				const string message = "Error retrieving response.  Check inner details for more info.";
				var exception = new ApplicationException(message, response.ErrorException);
				throw exception;
			}
			return response.Data;
		}

    }
}
