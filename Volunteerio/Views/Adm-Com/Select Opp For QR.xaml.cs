using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Select_Opp_For_QR : ContentPage
    {
        public Select_Opp_For_QR()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string Response = APIRequest.Request("MyOpps", true, new Dictionary<string, string>());

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
            try
            {
                string Code = APIRequest.Request("SignInStudents", true, new Dictionary<string, string>()
                {
                    {"OppId", (e.SelectedItem as Dictionary<string, string>)["ID"] }
                });

                Navigation.PushAsync(new Views.Sign_In_Students(Code));
            }
            catch (Exception ex)
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }


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
    }
}