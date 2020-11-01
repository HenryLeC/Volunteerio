using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void Request_Clicked(object sender, EventArgs e)
        {
            try
            {
                string response = APIRequest.Request("resetPassword", false, new Dictionary<string, string>
                {
                    {"uname", Uname.Text }
                });

                Dictionary<string, string> ResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                DisplayAlert(ResponseDict["title"], ResponseDict["msg"], "Ok");
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }
    }
}