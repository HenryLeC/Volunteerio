using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    public partial class Opportunity : ContentPage
    {
        readonly Dictionary<string, string> POpp = new Dictionary<string, string>();

        public Opportunity(Dictionary<string, string> Opp)
        {
            InitializeComponent();

            POpp = Opp;

            OppName.Text += Opp["Name"];
            OppDate.Text += Opp["Time"];
            OppLocation.Text += Opp["Location"];
            OppHours.Text += Opp["Hours"];
            OppSponsor.Text += Opp["Sponsor"];
            OppClass.Text += Opp["Class"];
            OppVols.Text = Opp["CurrentVols"] + " of " + Opp["MaxVols"] + "Volunteers";

            if (Int32.Parse(Opp["CurrentVols"]) >= Int32.Parse(Opp["MaxVols"]))
            {
                BookOppButton.IsEnabled = false;
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

                Navigation.PushAsync(new Views.Student_Menu());
            }
            catch
            {
                DisplayAlert("Server Error", "There Was A Server Error Please Try Again Later", "Ok");
            }

        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }
    }
}
