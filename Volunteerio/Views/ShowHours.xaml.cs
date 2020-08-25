using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowHours : ContentPage
    {

        private Dictionary<string, string> Hr = new Dictionary<string, string>();
        private Dictionary<string, string> StudentInfo = new Dictionary<string, string>();

        public ShowHours(Dictionary<string, string> Hr, Dictionary<string, string> StudentInfo)
        {
            InitializeComponent();
            this.Hr = Hr;
            this.StudentInfo = StudentInfo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StuNameL.Text += StudentInfo["Name"];
            HourReason.Text += Hr["Reason"];
            HourDesc.Text = Hr["Desc"];
            Hours.Text += Hr["Hours"];
            Confirmed.Text = Hr["Confirmed"];

            if (Hr["UnconfirmedBool"] == "True")
            {
                ConfirmBtn.IsVisible = true;
                DeleteBtn.IsVisible = true;
            }
        }

        private void HeaderControl_HamburgerButtonClicked(object source, EventArgs e)
        {
            Navigation.PopToRootAsync(false);
            Navigation.PushAsync(new Administrator_Menu());
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                StudentInfo["Hours"] = (int.Parse(StudentInfo["Hours"]) + int.Parse(Hr["Hours"])).ToString();

                APIRequest.Request("confirmHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", Hr["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }

        private void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("deleteHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", Hr["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }
    }
}