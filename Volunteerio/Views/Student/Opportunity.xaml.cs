using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Volunteerio.Views
{
    public partial class Opportunity : ContentPage
    {
        readonly Dictionary<string, string> POpp = new Dictionary<string, string>();

        public Opportunity(Dictionary<string, string> Opp, bool Bookable)
        {
            InitializeComponent();

            POpp = Opp;

            OppName.Text += Opp["Name"];
            OppDate.Text += Opp["Time"];
            OppHours.Text += Opp["Hours"];
            OppSponsor.Text += Opp["Sponsor"];
            OppClass.Text += Opp["Class"];
            OppVols.Text = Opp["CurrentVols"] + " of " + Opp["MaxVols"] + " Volunteers";
            OppLocation.Text += Opp["Location"];

            if (int.Parse(Opp["CurrentVols"]) >= int.Parse(Opp["MaxVols"]) || (string)Application.Current.Properties["Role"] == "admin")
            {
                BookOppButton.IsEnabled = false;
            }

            if (Bookable == false)
            {
                BookOppButton.IsVisible = false;
                UnBookOppButton.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string response = APIRequest.Request("OppInfo", true, new Dictionary<string, string>
                {
                    {"ID", POpp["ID"] }
                });

                Dictionary<string, string> OppInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                OppDesc.Text = OppInfo["Description"];

            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private void BookOppButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string response = APIRequest.Request("BookAnOpp", true, new Dictionary<string, string>()
                {
                    {"OppId", POpp["ID"] }
                });

                Dictionary<string, string> responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                DisplayAlert("Opp Booked", responseDict["msg"], "Ok");

                Navigation.PopAsync();
            }
            catch
            {
                DisplayAlert("Server Error", "There Was A Server Error Please Try Again Later", "Ok");
            }

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

        private void UnBookOppButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string response = APIRequest.Request("UnBookAnOpp", true, new Dictionary<string, string>()
                {
                    {"OppId", POpp["ID"] }
                });

                Dictionary<string, string> responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                DisplayAlert("Opp UnBooked", responseDict["msg"], "Ok");

                Navigation.PopAsync();
            }
            catch
            {
                DisplayAlert("Server Error", "There Was A Server Error Please Try Again Later", "Ok");
            }
        }

        private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
