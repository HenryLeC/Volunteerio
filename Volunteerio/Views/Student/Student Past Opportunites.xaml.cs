using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private async void ExportButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                aiStack.IsVisible = true;
                ai.IsRunning = true;

                string path = "";

                await Task.Run(() =>
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

                    path = Path.Combine(Xamarin.Forms.Application.Current.Properties["docsPath"] as string, "ExportedOpps.pdf");

                    File.WriteAllBytes(path, response.RawBytes);
                });

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(path, "application/pdf")
                });

                aiStack.IsVisible = false;
                ai.IsRunning = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                aiStack.IsVisible = false;
                ai.IsRunning = false;
                await DisplayAlert("Error", "Server Error, Please Try Again Later", "OK");
            }
        }
    }
}