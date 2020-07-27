using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_Opportunity_Info : ContentPage
    {

        readonly Dictionary<string, string> OpportunityInfo = new Dictionary<string, string>();

        public My_Opportunity_Info(Dictionary<string, string> OpportunityInfoPass)
        {
            OpportunityInfo = OpportunityInfoPass;

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
                string response = APIRequest.Request("BookedStudents", new Dictionary<string, string>()
                {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() },
                    {"OppId", OpportunityInfo["ID"] }
                });

                List<Dictionary<string, string>> students = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);

                BookedStudentsListView.ItemsSource = students;
                OppLocation.Text += OpportunityInfo["Location"];
                OppTime.Text += OpportunityInfo["Time"];
                OppName.Text += OpportunityInfo["Name"];
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
            
            base.OnAppearing();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool delete = await DisplayAlert("Delete Opportunity", "Are you sure you would like to delete this opportuniy?", "Delete", "Cancel");
            if (delete)
            {
                string response = APIRequest.Request("DeleteOpp", new Dictionary<string, string>
                {
                    { "x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() },
                    { "OppId", OpportunityInfo["ID"] },
                });

                if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "community")
                {
                    await Navigation.PushAsync(new Views.Community_Member_Menu());
                }
                else if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "admin")
                {
                    await Navigation.PushAsync(new Views.Administrator_Menu());
                }
            }
        }
    }
}