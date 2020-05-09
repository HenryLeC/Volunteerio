using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administrator_Menu : ContentPage
    {
        public Administrator_Menu()
        {
            InitializeComponent();
        }

        private void DashButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Dashboard());
        }

        private void CreatOppButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.AddAnOpp());
        }

        private void StudentsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Student_Search());
        }

        private void SignInStudentsButton_Clicked(object sender, EventArgs e)
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
            Navigation.PushAsync(new Views.Administrator_Menu());
        }

        private void NotificationsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Notifications());
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

        void LeaderboardButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Views.Leaderboard());
        }
    }
}