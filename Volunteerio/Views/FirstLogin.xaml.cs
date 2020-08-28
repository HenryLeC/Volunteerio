using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstLogin : ContentPage
    {

        string uName;

        public FirstLogin(string uName)
        {
            InitializeComponent();
            this.uName = uName;
        }

        private void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsEmailValid(EmailEntry.Text))
                {
                    APIRequest.Request("FirstSetup", true, new Dictionary<string, string>
                    {
                        {"password", PasswordEntry.Text },
                        {"email", EmailEntry.Text }
                    });

                    string role = Application.Current.Properties["Role"] as string;

                    UserInfo user = new UserInfo
                    {
                        Password = PasswordEntry.Text,
                        Username = uName,
                        Role = role,
                        Token = Application.Current.Properties["Token"] as string
                };
                    string fileName = "data.json";
                    string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
                    string path = Path.Combine(documentsPath, fileName);

                    File.WriteAllText(path, JsonConvert.SerializeObject(user));

                    if (role == "student")
                    {
                        Navigation.PushAsync(new Student_Menu());
                        Application.Current.MainPage = new NavigationPage(new Student_Menu());
                    }
                    else if (role == "community")
                    {
                        Navigation.PushAsync(new Community_Member_Menu());
                        Application.Current.MainPage = new NavigationPage(new Community_Member_Menu());
                    }
                    else if (role == "admin" || role == "teacher")
                    {
                        Navigation.PushAsync(new Administrator_Menu());
                        Application.Current.MainPage = new NavigationPage(new Administrator_Menu());
                    }

                    DisplayAlert("Confirm Your Email", "Please Confirm Your Email. You will recieve an email within a few minutes", "Ok");

                }
                else
                {
                    EmailEntry.BackgroundColor = Color.Red;
                }
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private bool IsEmailValid(string emailaddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}