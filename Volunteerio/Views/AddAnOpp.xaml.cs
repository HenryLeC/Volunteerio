using System;
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
            OppClass.ItemsSource = Classes.CLASSES;
        }

        private void AddOppButton_Clicked(object sender, EventArgs e)
        {

            string Name = OppName.Text;
            string Hours = OppHours.Text;
            string Location = OppLocation.Text;
            DateTime Date = OppDate.Date.Add(OppTime.Time);
            string Class = OppClass.SelectedItem as string;
            bool MaxVolsB = int.TryParse(OppMaxVols.Text, out int MaxVols);

            if (!MaxVolsB)
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
                    {"Location", Location },
                    {"Hours", Hours },
                    {"Class", Class },
                    {"Description", DescriptionEditor.Text },
                    {"MaxVols", MaxVols.ToString() }
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
    }
}