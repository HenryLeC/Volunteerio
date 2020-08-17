﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using RestSharp.Extensions;

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

            if (int.Parse(Opp["CurrentVols"]) >= int.Parse(Opp["MaxVols"]) || (string)Application.Current.Properties["Role"] == "admin" || Bookable == false)
            {
                BookOppButton.IsEnabled = false;
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

                OppLocation.Text += OppInfo["Location"];
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

                Navigation.PushAsync(new Views.Student_Menu());
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
    }
}
