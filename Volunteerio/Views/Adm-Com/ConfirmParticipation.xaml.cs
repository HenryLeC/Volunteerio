using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmParticipation : ContentPage
    {

        private readonly Dictionary<string, string> data;

        public ConfirmParticipation(Dictionary<string, string> data)
        {
            InitializeComponent();

            this.data = data;

            OppName.Text += data["OppName"];
            StuName.Text += data["Student"];
            HourComp.Text += data["StuCompleted"];
            HourTotal.Text += data["OppHours"];
        }

        private void HeaderControl_HamburgerButtonClicked(object source, EventArgs e)
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

        private void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("ConfParticipation", true, new Dictionary<string, string>() {
                    {"ID", data["ID"] },
                    {"Hours", HoursInput.Text }
                });
                Navigation.PopAsync();
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}