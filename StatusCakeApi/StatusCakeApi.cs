using RestSharp;
using System;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using StatusCakeApi.Models;
using NLog;


namespace StatusCakeApi
{

    public class StatusCakeApiClient	
    {
		const string BaseUrl = "https://api.statuscake.com/v1/";
		readonly RestClient client;
		readonly Logger _logger;

		public StatusCakeApiClient(string apiKey, Logger logger)
		{
			var options = new RestClientOptions(BaseUrl)
			{
				Authenticator = new JwtAuthenticator(apiKey)
			};

			client = new RestClient(options);
			
			_logger = logger;
		}

		public async Task<UptimeResults> GetFailedTestsAsync()
		{
			var request = new RestRequest("uptime", Method.Get);
			request.AddParameter("limit", 100, ParameterType.QueryString);
			request.AddParameter("status", "down", ParameterType.QueryString); // Only failed tests
			var response = await client.ExecuteAsync<UptimeResults>(request);

			if (!response.IsSuccessful)
			{
				var error = $"Exception occurred fetching failed tests from StatusCake: IsSuccessful: {response.IsSuccessful} StatusCode: {response.StatusCode} Response: {response.Content}";
				_logger.Fatal(error);
				throw new ApplicationException(error);
			}

			return response.Data;
		}

		public async Task<SslTestResults> GetSSLTestsAsync()
		{
			var request = new RestRequest("ssl");
			var result = await client.ExecuteAsync<SslTestResults>(request);

			if (!result.IsSuccessful)
			{
				var error = "Error fetching SSL test results: StatusCode: {result.StatusCode} Response: {result.Content}";
				_logger.Fatal(error);
				throw new ApplicationException(error);
			}

			return result.Data;
		}
    }
}
