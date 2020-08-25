using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Confirm_Opportunity : ContentPage
    {

        readonly Dictionary<string, string> POpp = new Dictionary<string, string>();

        public Confirm_Opportunity(Dictionary<String, String> Opp)
        {
            InitializeComponent();

            POpp = Opp;

            OppName.Text += Opp["Name"];
            OppDate.Text += Opp["Time"];
            OppLocation.Text += Opp["Location"];
            OppHours.Text += Opp["Hours"];
            OppSponsor.Text += Opp["Sponsor"];
            OppClass.Text += Opp["Class"];
            OppVols.Text += Opp["MaxVols"];
        }

        private void HeaderControl_HamburgerButtonClicked(object source, EventArgs e)
        {
            Navigation.PushAsync(new Volunteerio.Views.Administrator_Menu());
        }

        private void OptionButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("ConfDelOpp", true, new Dictionary<string, string>
                {
                    {"Mode", ((Button)sender).CommandParameter as String },
                    {"ID", POpp["ID"] }
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }

            Navigation.PushAsync(new Views.Administrator_Menu());
        }
    }
}