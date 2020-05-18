using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAnOpp : ContentPage
    {
        public AddAnOpp()
        {
            InitializeComponent();
            OppDate.MinimumDate = DateTime.Now;
        }

        private void AddOppButton_Clicked(object sender, EventArgs e)
        {

            string Name = OppName.Text;
            string Hours = OppHours.Text;
            string Location = OppLocation.Text;
            DateTime Date = OppDate.Date.Add(OppTime.Time);

            try
            {

                Dictionary<string, string> Attributes = new Dictionary<string, string>()
                {
                    {"x-access-token", Xamarin.Forms.Application.Current.Properties["Token"].ToString() },
                    {"Name", Name },
                    {"Date", Date.ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture) },
                    {"Location", Location },
                    {"Hours", Hours }
                };

                string response = APIRequest.Request("AddOpp", Attributes);
                
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
                ErrorLabel.Text = "Server Error, Please Try Again Later";
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