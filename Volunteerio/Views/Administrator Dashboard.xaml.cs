using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Newtonsoft.Json;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administrator_Dashboard : ContentPage
    {
        public Administrator_Dashboard()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                string Response = APIRequest.Request("MyOpps", new Dictionary<string, string>() {
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

            base.OnAppearing();

        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Menu());
        }

        private void OppsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Views.My_Opportunity_Info(e.SelectedItem as Dictionary<string, string>));
        }
    }
}