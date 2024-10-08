﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Volunteerio.Views
{
    public partial class Leaderboard : ContentPage
    {
        public Leaderboard()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            FilterDrop.ItemsSource = new List<string>(2)
            {
                "District", "School"
            };
            FilterDrop.SelectedItem = "District";

            try
            {
                string request = APIRequest.Request("Leaderboard", true, new Dictionary<string, string>() {
                    {"filter", "district" }
                });

                List<Dictionary<string, string>> leaderboard = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request);
                leaderboardListView.ItemsSource = leaderboard;
            }
            catch (Exception)
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }


            base.OnAppearing();
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

        private void FilterDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string request = APIRequest.Request("Leaderboard", true, new Dictionary<string, string>() {
                    {"filter", FilterDrop.SelectedItem.ToString().ToLower() }
                });

                List<Dictionary<string, string>> leaderboard = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request);
                leaderboardListView.ItemsSource = leaderboard;
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }
    }
}
