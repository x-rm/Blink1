using RestSharp;
using RestSharp.Authenticators;

namespace StatusCakeApi
{
	internal class StatusCakeAuthenticator : IAuthenticator
	{
		private string username;
		private string apiKey;

		public StatusCakeAuthenticator(string username, string apiKey)
		{
			this.username = username;
			this.apiKey = apiKey;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.AddHeader("Username", username);
			request.AddHeader("API", apiKey);
		}
	}
}