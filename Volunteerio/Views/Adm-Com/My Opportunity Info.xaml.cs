﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_Opportunity_Info : ContentPage
    {

        readonly Dictionary<string, string> Opp = new Dictionary<string, string>();

        public My_Opportunity_Info(Dictionary<string, string> OpportunityInfoPass)
        {
            Opp = OpportunityInfoPass;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                string response = APIRequest.Request("BookedStudents", true, new Dictionary<string, string>()
                {
                    {"OppId", Opp["ID"] }
                });

                List<Dictionary<string, string>> students = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);

                if (Xamarin.Forms.Application.Current.Properties["Role"].ToString() == "admin")
                {
                    OppConf.IsVisible = false;
                }

                BookedStudentsListView.ItemsSource = students;
                OppConf.Text += Opp["Confirmed"];
                OppName.Text += Opp["Name"];
                OppDate.Text += Opp["Time"];
                OppLocation.Text += Opp["Location"];
                OppHours.Text += Opp["Hours"];
                OppSponsor.Text += Opp["Sponsor"];
                OppClass.Text += Opp["Class"];
                OppVols.Text = Opp["CurrentVols"] + " of " + Opp["MaxVols"] + " Volunteers";

                SwipeGestureRecognizer swipe = new SwipeGestureRecognizer() { Direction = SwipeDirection.Right };
                swipe.Swiped += SwipeRight_Swiped;

                if (Device.RuntimePlatform == Device.Android)
                {
                    ScrollView.GestureRecognizers.Add(swipe);
                    BookedStudentsListView.GestureRecognizers.Add(swipe);
                }
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }

            base.OnAppearing();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool delete = await DisplayAlert("Delete Opportunity", "Are you sure you would like to delete this opportuniy?", "Delete", "Cancel");
                if (delete)
                {
                    string response = APIRequest.Request("DeleteOpp", true, new Dictionary<string, string>
                    {
                        { "OppId", Opp["ID"] },
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
            catch
            {
                await DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
        {
            Console.WriteLine("**** " + sender.ToString());
            Navigation.PopAsync();
        }
    }
}