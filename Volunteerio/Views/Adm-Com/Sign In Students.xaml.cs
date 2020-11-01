using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sign_In_Students : ContentPage
    {
        public Sign_In_Students(string Code)
        {
            InitializeComponent();

            Barcode.BarcodeOptions = new ZXing.Common.EncodingOptions() { Height = 300, Width = 300 };
            Barcode.BarcodeFormat = BarcodeFormat.QR_CODE;
            Barcode.BarcodeValue = Code;
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

        private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}