using System;
using System.IO;
using System.Net;
using System.Object;
using Newtonsoft.Json;

namespace funwaps.Web
{
    class client : HttpClient 
    {

        public client (){

        }
        
        public void sendRequest( string URI , string json)
        {
            WebRequest request = WebRequest.Create(URI);
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription); //displat the status


        }

    }
}
