using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APproject
{
	public static class HelperHttpClient
	{
		public static Task<HttpResponseMessage> PostAsyncRequest (string url, string json){
			var client = new HttpClient ();
			var content = new StringContent (json);
			return client.PostAsync (url, content);
		}

		public static object WaitResult (Task<HttpResponseMessage> task){
			var httpResponse = task.Result;
			var resultString = httpResponse.Content.ReadAsStringAsync().Result;

			int intRes;
			bool boolRes;
			if (int.TryParse (resultString, out intRes))
				return intRes;
			else if (bool.TryParse (resultString, out boolRes))
				return boolRes;
			else
				throw new DasyncException (resultString);
		}
	}
}

