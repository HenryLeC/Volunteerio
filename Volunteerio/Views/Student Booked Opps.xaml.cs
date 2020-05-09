using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Student_Booked_Opps : ContentPage
    {
        public Student_Booked_Opps()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string Response = APIRequest.Request("BookedOpps", new Dictionary<string, string>() {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() }
                });

                //Parse Data
                List<Dictionary<string, string>> Opps = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Response);

                OppsListView.ItemsSource = Opps;
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
    }
}