using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inbox : ContentPage
    {
        public Inbox()
        {
            InitializeComponent();
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "community")
            {
                Navigation.PushAsync(new Views.Community_Member_Menu());
            }
            else if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "admin")
            {
                Navigation.PushAsync(new Views.Administrator_Menu());
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                string response = APIRequest.Request("Notifications", true, new Dictionary<string, string>());
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
            if ((e.SelectedItem as Dictionary<String, String>)["Type"] == "Hour")
            {
                Navigation.PushAsync(new Views.Administrator_Student_Info(e.SelectedItem as Dictionary<string, string>));
            }
            else if ((e.SelectedItem as Dictionary<String, String>)["Type"] == "Opportunity")
            {
                Navigation.PushAsync(new Views.Confirm_Opportunity(e.SelectedItem as Dictionary<String, String>));
            }
            else if ((e.SelectedItem as Dictionary<String, String>)["Type"] == "IncompleteOpp")
            {
                Navigation.PushAsync(new Views.ConfirmParticipation(e.SelectedItem as Dictionary<String, String>));
            }
        }
    }
}