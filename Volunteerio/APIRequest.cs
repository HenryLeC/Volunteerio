using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Volunteerio
{
    class APIRequest
    {

        //public static string server = "http://10.30.20.228/";
        //public static string server = "http://192.168.86.33/";
        public static string server = "https://api.volunteerio.us/";

        public static string Request(string Route, Dictionary<string, string> Parameters)
        {
            int ErrorCode = 0;

            try
            {
                //Create Client and Request
                var client = new RestClient(server + Route)
                {
                    Timeout = 5000
                };
                var request = new RestRequest(Method.POST)
                {
                    AlwaysMultipartFormData = true
                };

                //Add Parameters
                foreach (KeyValuePair<string, string> Attribute in Parameters)
                {
                    request.AddParameter(Attribute.Key, Attribute.Value);
                }

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ErrorCode = 500;
                    throw new Exception();
                }
                return response.Content;
            }
            catch
            {
                throw new ServerErrorException(ErrorCode);
            }
        }
    }
}
