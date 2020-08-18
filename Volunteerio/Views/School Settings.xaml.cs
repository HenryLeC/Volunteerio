using Newtonsoft.Json;
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
    public partial class School_Settings : ContentPage
    {
        public School_Settings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string resp = APIRequest.Request("schoolSettings", true, new Dictionary<string, string>());

            Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp);

            HoursGoal.Text = settings["SGoal"];
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            HoursGoal.IsReadOnly = !HoursGoal.IsReadOnly;
        }

        private void HoursGoal_Completed(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("schoolGoal", true, new Dictionary<string, string> {
                    {"Goal", ((Entry)sender).Text }
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }
    }
}