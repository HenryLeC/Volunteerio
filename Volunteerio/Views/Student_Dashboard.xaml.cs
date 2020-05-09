using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    public partial class Student_Dashboard : ContentPage
    {
        public Student_Dashboard()
        {
            InitializeComponent();

            try
            {
                string response = APIRequest.Request("hours", new Dictionary<string, string>()
                {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() }
                });

                Dictionary<string, string> HoursDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                HoursText.Text = HoursDict["hours"];
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            HoursImage.RotateTo(365, 1000);
            HoursImage.Rotation = 0;
        }

        private void AddHoursButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (HoursEntry.Text != "" && ReasonEntry.Text != "" && Int32.TryParse(HoursEntry.Text, out int HoursInt))
                {

                    APIRequest.Request("addhours", new Dictionary<string, string>()
                    {
                        {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() },
                        {"hours", HoursEntry.Text },
                        {"reason", ReasonEntry.Text }
                    });

                    HoursEntry.Text = "";
                    ReasonEntry.Text = "";

                    DisplayAlert("Hours Added", "Your Hours Have Been Added, Please Wait For Them To Be Confirmed", "Ok");
                }
                else
                {
                    DisplayAlert("Invalid", "Please Enter a Number For Hours And Text For Reason", "Ok");
                }

            }
            catch (ServerErrorException)
            {

                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }

        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SafeAreaInsets")
            {
                On<iOS>().SetUseSafeArea(false);
                Thickness Insets = On<iOS>().SafeAreaInsets();
                InsertsRow.Height = Insets.Top;

            }
        }
    }
}
