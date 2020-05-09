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
                string Response = APIRequest.Request("PastOpps", new Dictionary<string, string>() {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() }
                });

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

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SafeAreaInsets")
            {
                On<iOS>().SetUseSafeArea(false);
                Thickness Insets = On<iOS>().SafeAreaInsets();
                InsertsRow.Height = Insets.Top;

            }
        }

        private void ExportButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string response = APIRequest.Request("GenerateDoc", new Dictionary<string, string>()
                {
                    { "x-access-token", Xamarin.Forms.Application.Current.Properties["Token"] as String }
                });

                //Parse Data
                Dictionary<string, string> Message = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                DisplayAlert("Email Sent", Message["msg"], "Ok");
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
        }
    }
}