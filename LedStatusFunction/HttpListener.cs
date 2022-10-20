using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using StatusCakeApi;
using StatusCakeApi.Models;

namespace LedStatusFunction
{
    public static class HttpListener
    {
	    public static int cachedCount = -1;
	    public static DateTime lastFetch = DateTime.MinValue;
	    public static ILogger logger;

	    [Function("HttpListener")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
	        logger = executionContext.GetLogger("HttpListener");

	        if (lastFetch > DateTime.Now.AddSeconds(-30) && cachedCount != -1)
	        {
				logger.Log(LogLevel.Information, "Returning cached value from " + lastFetch.ToString("s"));
				var secondsAgo = (int)(DateTime.Now - lastFetch).TotalSeconds;
				var response = GetResponseData(req, cachedCount, $"cached {secondsAgo} seconds ago");
				return response;
	        }

	        try
	        {
		        logger.LogInformation("Returning non-cached value");

		        var apiKey = Environment.GetEnvironmentVariable("StatusCakeApiKey");

		        if (String.IsNullOrEmpty(apiKey))
		        {
			        var badResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
			        badResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
			        badResponse.WriteString("Error: StatusCakeApiKey environment variable was not set");
			        return badResponse;
		        }

		        StatusCakeApiClient _api = new StatusCakeApiClient(apiKey);

		        var failedTests = _api.GetFailedTests();
		        cachedCount = failedTests.Data.Count;
		        lastFetch = DateTime.Now;

		        var responseData = GetResponseData(req, cachedCount, "new value");
		        return responseData;
	        }
	        catch (Exception ex)
	        {
				logger.Log(LogLevel.Critical, "Exception: " + ex.ToString());
				throw;
	        }
	        
        }

        private static HttpResponseData GetResponseData(HttpRequestData req, int failedTests, string text)
        {
	        var response = req.CreateResponse(HttpStatusCode.OK);
	        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

	        response.WriteString("Result:" + failedTests + "|" + text);
	        return response;
        }

    }
}
