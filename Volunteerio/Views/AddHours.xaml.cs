using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddHours : ContentPage
    {
        public AddHours()
        {
            InitializeComponent();
        }

        private void AddHours_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (HoursEntry.Text != "" && ReasonEntry.Text != "" && DescEditor.Text != "" && Int32.TryParse(HoursEntry.Text, out int HoursInt))
                {

                    APIRequest.Request("addhours", true, new Dictionary<string, string>()
                    {
                        {"hours", HoursEntry.Text },
                        {"reason", ReasonEntry.Text },
                        {"desc", DescEditor.Text }
                    });

                    HoursEntry.Text = "";
                    ReasonEntry.Text = "";

                    DisplayAlert("Hours Added", "Your Hours Have Been Added, Please Wait For Them To Be Confirmed", "Ok");
                }
                else
                {
                    DisplayAlert("Invalid", "Please Enter a Number For Hours, Text For Description And Text For Reason", "Ok");
                }
                Navigation.PopAsync(true);
            }
            catch (ServerErrorException)
            {

                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
        }

        private void HeaderControl_HamburgerButtonClicked(object source, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }
    }
}