using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.IO;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Community_Member_Menu : ContentPage
    {
        public Community_Member_Menu()
        {
            InitializeComponent();
        }

        private void DashButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Community_Member_Dashboard());
        }

        private void AddAnOppButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.AddAnOpp());
        }

        private void SignInStudents_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Select_Opp_For_QR());
        }

        private void SignOutButton_Clicked(object sender, EventArgs e)
        {
            string fileName = "data.json";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            string path = Path.Combine(documentsPath, fileName);
            File.Delete(path);

            Xamarin.Forms.Application.Current.Properties["Role"] = null;
            Xamarin.Forms.Application.Current.Properties["Username"] = null;
            Xamarin.Forms.Application.Current.Properties["Password"] = null;
            Xamarin.Forms.Application.Current.Properties["Token"] = null;
            Navigation.PushAsync(new Views.Login());
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Community_Member_Menu());
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SafeAreaInsets")
            {
                On<iOS>().SetUseSafeArea(false);
                Thickness Insets = On<iOS>().SafeAreaInsets();
                InsertsRow.Height = Insets.Top;

            }
        }
    }
}