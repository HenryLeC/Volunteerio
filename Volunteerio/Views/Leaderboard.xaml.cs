﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Newtonsoft.Json;

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
            try
            {
                string request = APIRequest.Request("Leaderboard", new Dictionary<string, string>() {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() }
                });

                List<Dictionary<string, string>> leaderboard = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request);
                leaderboardListView.ItemsSource = leaderboard;
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            

            base.OnAppearing();
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            if(Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "student")
            {
                Navigation.PushAsync(new Views.Student_Menu());
            }
            else if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "admin")
            {
                Navigation.PushAsync(new Views.Administrator_Menu());
            }
            
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SafeAreaInsets")
            {
                On<iOS>().SetUseSafeArea(false);
                Thickness Insets = On<iOS>().SafeAreaInsets();
                InsertsRow.Height = Insets.Top;

            }
        }

    }
}
