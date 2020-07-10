using System;
using System.Collections.Generic;
using RestSharp;

namespace Volunteerio
{
    class APIRequest
    {
        public static string Request(string Route, Dictionary<string, string> Parameters)
        {
            int ErrorCode = 0;

            try
            {
                //string server = "http://73.57.34.161/";
                //string server = "http://192.168.86.25/";
                string server = "https://api.volunteerio.us/";

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

                if (response.Content == null)
                {
                    ErrorCode = 500;
                    throw new Exception();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ErrorCode = 401;
                    throw new Exception();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ErrorCode = 500;
                    throw new Exception();
                }
                return response.Content;

                //Parse Response
                 //return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            }
            catch
            {
                throw new ServerErrorException(ErrorCode);
            }
        }
    }
}
