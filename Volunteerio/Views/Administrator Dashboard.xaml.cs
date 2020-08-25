using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                string Response = APIRequest.Request("MyOpps", true, new Dictionary<string, string>());

                //Parse Data
                List<Dictionary<string, string>> Opps = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Response);

                string Response2 = APIRequest.Request("UnconfOpps", true, new Dictionary<string, string>());

                //Parse Data
                List<Dictionary<string, string>> UnconfOpps = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Response2);

                OppsListView.ItemsSource = Opps;
                UnConfOppsListView.ItemsSource = UnconfOpps;
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

        private void UnConfOppsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Views.Confirm_Opportunity(e.SelectedItem as Dictionary<string, string>));
        }
    }
}