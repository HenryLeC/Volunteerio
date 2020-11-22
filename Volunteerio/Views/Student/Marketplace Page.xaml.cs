using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Marketplace_Page : ContentPage
    {
        public Marketplace_Page()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DateFilter.MinimumDate = DateTime.Now;
            //if (Device.RuntimePlatform == Device.iOS && Xamarin.Essentials.DeviceInfo.VersionString == "14.0")
            //{
            //    DateFilter.IsEnabled = false;
            //    DateFilter.Date = DateTime.Now;
            //}

            FillOpps();
        }

        private void OppsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Opportunity(e.SelectedItem as Dictionary<string, string>, true));
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "student")
            {
                Navigation.PushAsync(new Views.Student_Menu());
            }
            else if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "admin")
            {
                Navigation.PushAsync(new Views.Administrator_Menu());
            }
        }

        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillOpps();
        }

        private void DateFilter_DateSelected(object sender, DateChangedEventArgs e)
        {
            FillOpps();
        }

        private async void FillOpps()
        {
            try
            {
                string Response = await APIRequest.RequestAsync("Opps", true, new Dictionary<string, string>() {
                    {"filterName", NameFilter.Text },
                    {"filterDate", DateFilter.Date.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture) }
                });

                //Parse Data
                List<Dictionary<string, string>> Opps = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Response);

                OppsListView.ItemsSource = Opps;
            }
            catch
            {
                await DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
        }
    }
}