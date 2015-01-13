using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APproject
{
	public static class HelperHttpClient
	{
		public static Task<HttpResponseMessage> PostAsyncRequest (string url, string json){
			try{
				var client = new HttpClient ();
				var content = new StringContent (json);
				return client.PostAsync (url, content);
			}catch(HttpRequestException e){
				throw new ServerConnectionException (e.Message);
			}
		}

		public static object WaitResult (Task<HttpResponseMessage> task){
			try {
				var httpResponse = task.Result;
				var resultString = httpResponse.Content.ReadAsStringAsync ().Result;

				int intRes;
				bool boolRes;
				if (int.TryParse (resultString, out intRes))
					return intRes;
				else if (bool.TryParse (resultString, out boolRes))
					return boolRes;
				else
					throw new DasyncException (resultString);
			} catch (AggregateException e) {
				throw new ServerConnectionException (e.Message);
			}
		}
	}
}

