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
                string Response = APIRequest.Request("BookedOpps", true, new Dictionary<string, string>());

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

        private void OppsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Opportunity(e.SelectedItem as Dictionary<string, string>, false));
        }
    }
}