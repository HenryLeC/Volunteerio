using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace Volunteerio.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clock_Page : ContentPage
    {
        public Clock_Page()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void CodeEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CodeEntry.Text.Length == 6)
            {
                Dictionary<string, string> response = await APIRequest<Dictionary<string, string>>.RequestAsync("ClockInOut", true, new Dictionary<string, string>()
                {
                    {"QrCode", CodeEntry.Text }
                });

                await DisplayAlert(response["header"], response["msg"], "OK");
            }
        }

#pragma warning disable IDE0051 // Remove unused private members
        private async void Scanner_OnScanResult(ZXing.Result result)
#pragma warning restore IDE0051 // Remove unused private members
        {
            try
            {
                Dictionary<string, string> response = await APIRequest<Dictionary<string, string>>.RequestAsync("ClockInOut", true, new Dictionary<string, string>()
                {
                    {"QrCode", result.Text }
                });

                await DisplayAlert(response["header"], response["msg"], "OK");
            }
            catch
            {
                await DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
        }
    }
}