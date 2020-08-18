using System;
using System.IO;
using Xamarin.Forms;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties["Role"] as string == "teacher")
            {
                SchoolSettingsButton.IsVisible = false;
            }
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
            string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
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

        void LeaderboardButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Views.Leaderboard());
        }

        private void InboxButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Inbox());
        }

        private void MarketplaceButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Marketplace_Page());
        }

        private void SchoolSettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.School_Settings());
        }
    }
}