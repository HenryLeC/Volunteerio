using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using RestSharp;
using System.Net;
using System.IO;
using Xamarin.Essentials;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Student_Past_Opportunites : ContentPage
    {
        public Student_Past_Opportunites()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string Response = APIRequest.Request("PastOpps", true, new Dictionary<string, string>());

                //Parse Data
                Dictionary<string, List<Dictionary<string, string>>> Opps = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(Response);

                PastOppsListView.ItemsSource = Opps["PastOpps"];
                PastHoursListView.ItemsSource = Opps["Hours"];
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
            
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }

        private void ExportButton_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    string response = APIRequest.Request("GenerateDoc", new Dictionary<string, string>()
            //    {
            //        { "x-access-token", Xamarin.Forms.Application.Current.Properties["Token"] as String }
            //    });

            //    //Parse Data
            //    Dictionary<string, string> Message = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            //    DisplayAlert("Email Sent", Message["msg"], "Ok");
            //}
            //catch
            //{
            //    DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            //}

            try
            {
                //Create Client and Request
                var client = new RestClient(APIRequest.server + "GenerateDoc")
                {
                    Timeout = 5000
                };
                var request = new RestRequest(Method.POST)
                {
                    AlwaysMultipartFormData = true
                };
                request.AddParameter("x-access-token", Xamarin.Forms.Application.Current.Properties["Token"] as string);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();
                }

                string path = Path.Combine(Xamarin.Forms.Application.Current.Properties["docsPath"] as string, "ExportedOpps.pdf");
                string path2 = "";
                foreach (char s in path)
                {
                    if (s == '\\')
                    {
                        path2 += "/";
                    }
                    else
                    {
                        path2 += s.ToString();
                    }
                }

                File.WriteAllBytes(path, response.RawBytes);
                Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(path, "application/pdf")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                DisplayAlert("Error", "Server Error, Please Try Again Later", "OK");
            }
        }
    }
}