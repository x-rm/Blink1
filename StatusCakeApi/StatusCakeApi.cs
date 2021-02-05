using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Serializers.NewtonsoftJson;

namespace StatusCakeApi
{

    public class StatusCakeApiClient	
    {
		const string BaseUrl = "https://app.statuscake.com/API/";
		readonly IRestClient client;

		public StatusCakeApiClient(string username, string apiKey)
		{
			client = new RestClient(BaseUrl);
			client.Authenticator = new StatusCakeAuthenticator(username, apiKey);
			client.UseNewtonsoftJson();
		}

		public List<TestDto> GetTests()
		{
			var request = new RestRequest("Tests");
			var tests = client.Get<List<TestDto>>(request);
			return tests.Data;
		}

		public List<SslDto> GetSSLTests()
		{
			var request = new RestRequest("SSL");
			var tests = client.Get<List<SslDto>>(request);
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
