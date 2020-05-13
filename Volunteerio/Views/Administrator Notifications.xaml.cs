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
    public partial class Administrator_Notifications : ContentPage
    {
        public Administrator_Notifications()
        {
            InitializeComponent();
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Menu());
        }

        protected override void OnAppearing()
        {
            try
            {
                string response = APIRequest.Request("Notifications", new Dictionary<string, string>()
                {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() }
                });
                List<Dictionary<string, string>> responseList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);
                NotificationsListView.ItemsSource = responseList;
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Agian Later", "Ok");
            }
            
            base.OnAppearing();
        }

        private void NotificationsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Student_Info(e.SelectedItem as Dictionary<string, string>));
        }
    }
}