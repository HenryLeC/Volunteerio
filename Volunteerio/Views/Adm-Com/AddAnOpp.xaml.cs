﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAnOpp : ContentPage
    {
        public AddAnOpp()
        {
            InitializeComponent();
            OppDate.MinimumDate = DateTime.Now;
            //if (Device.RuntimePlatform == Device.iOS && Xamarin.Essentials.DeviceInfo.VersionString == "14.0")
            //{
            //    OppTime.IsEnabled = false;
            //    OppDate.IsEnabled = false;

            //    DisplayAlert("Unsuported iOS Version", "iOS 14 is not currently supported for creating opportunities.", "OK");
            //}
            OppClass.ItemsSource = Classes.CLASSES;
        }

        private void AddOppButton_Clicked(object sender, EventArgs e)
        {
            //if (Device.RuntimePlatform == Device.iOS && Xamarin.Essentials.DeviceInfo.VersionString == "14.0")
            //{

            //    DisplayAlert("Unsuported iOS Version", "iOS 14 is not currently supported for creating opportunities.", "OK");
            //    return;
            //}

            string Name = OppName.Text;
            string Location = OppLocation.Text;
            DateTime Date = OppDate.Date.Add(OppTime.Time);
            string Class = OppClass.SelectedItem as string;
            bool MaxVolsB = int.TryParse(OppMaxVols.Text, out int MaxVols);
            bool HoursB = int.TryParse(OppHours.Text, out int Hours);

            if (!HoursB)
            {
                DisplayAlert("Error", "Enter a number for Hours", "Ok");
                return;
            }
            else if (!MaxVolsB)
            {
                DisplayAlert("Error", "Enter a number for Maximum Volunteers", "Ok");
                return;
            }

            try
            {

                Dictionary<string, string> Attributes = new Dictionary<string, string>()
                {
                    {"Name", Name },
                    {"Date", Date.ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture) },
                    {"Location", OppLocation.IsVisible ? Location: "" },
                    {"Hours", Hours.ToString() },
                    {"Class", Class },
                    {"Description", DescriptionEditor.Text },
                    {"MaxVols", MaxVols.ToString() },
                    {"Virtual", VirtualSelector.IsToggled ? "True" : "False" }
                };

                string response = APIRequest.Request("AddOpp", true, Attributes);

                //Leave Page
                if ((string)Xamarin.Forms.Application.Current.Properties["Role"] == "admin")
                {
                    Navigation.PushAsync(new Views.Administrator_Menu());
                }
                else
                {
                    Navigation.PushAsync(new Views.Community_Member_Menu());
                }
            }
            catch
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

        private void VirtualSelector_Toggled(object sender, ToggledEventArgs e)
        {
            OppLocation.IsVisible = !OppLocation.IsVisible;
        }
    }
}