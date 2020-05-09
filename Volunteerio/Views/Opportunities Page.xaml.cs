using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Opportunities_Page : ContentPage
    {
        public Opportunities_Page()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string Response = APIRequest.Request("Opps", new Dictionary<string, string>() {
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

        private void OppsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Opportunity(e.SelectedItem as Dictionary<string, string>));
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