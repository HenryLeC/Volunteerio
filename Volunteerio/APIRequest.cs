using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Volunteerio
{
    class APIRequest<T>
    {

        public static string server = "http://192.168.86.57/api/";
        //public static string server = "https://www.volunteerio.us/api/";

        public async static Task<T> RequestAsync(string Route, bool Token, Dictionary<string, string> Paramters)
        {
            T val = await Task.Run(() => Request(Route, Token, Paramters));
            return val;
        }

        public static T Request(string Route, bool Token, Dictionary<string, string> Parameters)
        {
            int ErrorCode = 0;

            try
            {
                //Create Client and Request
                var client = new RestClient(server + Route)
                {
                    Timeout = 5000
                };
                RestRequest request = new RestRequest(Method.POST)
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

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)response.Content;
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ErrorCode);
            }
        }
    }

    class APIRequest : APIRequest<string> { }
}
