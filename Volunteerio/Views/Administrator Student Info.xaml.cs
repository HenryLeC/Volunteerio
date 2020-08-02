using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administrator_Student_Info : ContentPage
    {
        private Dictionary<string, string> StudentInfo { get; set; }

        public Administrator_Student_Info(Dictionary<string, string> Student)
        {
            InitializeComponent();

            StudentInfo = Student;

            try
            {
                string response = APIRequest.Request("StudentHours", true, new Dictionary<string, string>()
                {
                    {"id", Student["ID"] }
                });
                Dictionary<string, List<Dictionary<string, string>>> StudentHours = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(response);

                StudentName.Text += StudentInfo["Name"];
                StudentId.Text += StudentInfo["StuId"];
                Hours.Text += StudentInfo["Hours"];

                PastOppsListView.ItemsSource = StudentHours["PastOpps"];
                HoursListView.ItemsSource = StudentHours["Hours"];

                if (StudentHours["PastOpps"].Count == 0)
                {
                    PastOppsListView.HeightRequest = 50;
                }
                if (StudentHours["Hours"].Count == 0)
                {
                    HoursListView.HeightRequest = 50;
                }
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
            
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Administrator_Menu());
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StudentInfo["Hours"] = (int.Parse(StudentInfo["Hours"]) + int.Parse((((Button)sender).CommandParameter as Dictionary<string, string>)["Hours"])).ToString();

                APIRequest.Request("confirmHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", (((Button)sender).CommandParameter as Dictionary<string,string>)["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("deleteHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", (((Button)sender).CommandParameter as Dictionary<string,string>)["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }
    }

}