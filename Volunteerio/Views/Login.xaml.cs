using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            // Setup DocsPath for rest of app
            string documentsPath;
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData); //AppData Folder
                    break;
                default:
                    documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                    break;
            }
            Xamarin.Forms.Application.Current.Properties["docsPath"] = documentsPath;
        }

        private void LogInButton_Clicked(object sender, EventArgs e)
        {
            string Uname = UsernameField.Text;
            string Pass = PasswordField.Text;

            try
            {
                string Response = APIRequest.Request("login", false, new Dictionary<string, string>() {
                    {"username", Uname },
                    {"password", Pass }
                });

                //Parse Response
                Dictionary<string, string> ResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Response);
                string role = ResponseDict["role"];
                bool.TryParse(ResponseDict["firstTime"], out bool firstTime);

                if (!firstTime)
                {
                    UserInfo user = new UserInfo
                    {
                        Password = Pass,
                        Username = Uname,
                        Role = role,
                        Token = ResponseDict["key"]
                    };
                    string fileName = "data.json";
                    string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
                    string path = Path.Combine(documentsPath, fileName);

                    File.WriteAllText(path, JsonConvert.SerializeObject(user));
                }

                if (firstTime)
                {
                    Navigation.PushAsync(new Views.FirstLogin(Uname));
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.FirstLogin(Uname));
                }
                else if (role == "student")
                {
                    Navigation.PushAsync(new Views.Student_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.Student_Menu());
                }
                else if (role == "community")
                {
                    Navigation.PushAsync(new Views.Community_Member_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.Community_Member_Menu());
                }
                else if (role == "admin" || role == "teacher")
                {
                    Navigation.PushAsync(new Views.Administrator_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.Administrator_Menu());
                }
                else { }
            }
            catch (Exception ex)
            {
                if (ex == null)
                {
                    DisplayAlert("Server Error", "Please Try Agin Later", "OK");
                }
                if ((ex as ServerErrorException).ErrorCode == 401)
                {
                    DisplayAlert("Incorrect Login Details", "The Username or Pasword you enetered is incorrect.", "Ok");
                }
                else
                {
                    DisplayAlert("Server Error", "Please Try Agin Later", "OK");
                }

            }

        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SafeAreaInsets")
            {
                On<iOS>().SetUseSafeArea(false);
                Thickness Insets = On<iOS>().SafeAreaInsets();
                string fileName = "topInsert.txt";
                string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
                string path = Path.Combine(documentsPath, fileName);

                File.WriteAllText(path, Insets.Top.ToString());
                InsertsRow.Height = Insets.Top;

            }
        }

        private void ResetPassword_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.ResetPassword());
        }
    }
}