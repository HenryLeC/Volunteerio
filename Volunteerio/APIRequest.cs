using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Xamarin.Forms;

namespace Volunteerio
{
    class APIRequest
    {

        //public static string server = "http://10.30.20.228/";
        public static string server = "http://192.168.86.25/api/";
        //public static string server = "https://volunteerio.us/api/";

        public static string Request(string Route, bool Token, Dictionary<string, string> Parameters)
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
                if (Token)
                {
                    request.AddParameter("x-access-token", Application.Current.Properties["Token"] as string);
                }

                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized && !Token)
                {
                    ErrorCode = 401;
                    throw new Exception();
                }
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
