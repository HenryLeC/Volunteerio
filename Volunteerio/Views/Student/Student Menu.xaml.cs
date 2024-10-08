﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Volunteerio.Views.Student;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Student_Menu : ContentPage
    {
        public Student_Menu()
        {
            InitializeComponent();
        }

        private void DashboardButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Dashboard());
        }

        private void PastOppsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Past_Opportunites());
        }

        private void SignOutButton_Clicked(object sender, EventArgs e)
        {
            string fileName = "data.json";
            string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
            string path = Path.Combine(documentsPath, fileName);
            File.Delete(path);

            Xamarin.Forms.Application.Current.Properties["Role"] = null;
            Xamarin.Forms.Application.Current.Properties["Username"] = null;
            Xamarin.Forms.Application.Current.Properties["Password"] = null;
            Xamarin.Forms.Application.Current.Properties["Token"] = null;
            Application.Current.MainPage = new NavigationPage(new Views.Login());
            Navigation.PopToRootAsync();
        }

        private async void ScanQrCodeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Clock_Page());

            //#if __ANDROID__
            // // Initialize the scanner first so it can track the current context
            // MobileBarcodeScanner.Initialize (Application);
            //#endif

            //var scanPage = new ZXingScannerPage();
            //NavigationPage.SetHasNavigationBar(scanPage, true);


            //await Navigation.PushAsync(scanPage);

            //scanPage.OnScanResult += (result) =>
            //{
            //    // Stop scanning
            //    scanPage.IsScanning = false;

            //    // Pop the page and show the result
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Navigation.PopAsync();
            //        try
            //        {
            //            string APIResponse = APIRequest.Request("ClockInOut", true, new Dictionary<string, string>()
            //            {
            //                {"QrCode", result.Text }
            //            });

            //            Dictionary<string, string> responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(APIResponse);

            //            await DisplayAlert(responseDict["header"], responseDict["msg"], "OK");
            //        }
            //        catch
            //        {
            //            await DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            //        }
            //    });
            //};
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }

        private void BookedOppsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Booked_Opps());
        }

        void LeaderboardButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Views.Leaderboard());
        }

        private void MarketplaceButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Marketplace_Page());
        }
    }
}