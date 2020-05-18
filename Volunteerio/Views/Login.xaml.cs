using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.IO;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(false);
            Thickness Insets = On<iOS>().SafeAreaInsets();
            InsertsRow.Height = Insets.Top;
            string fileName = "topInsert.txt";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            string path = Path.Combine(documentsPath, fileName);

            File.WriteAllText(path, Insets.Top.ToString());
        }

        private void LogInButton_Clicked(object sender, EventArgs e)
        {
            string Uname = UsernameField.Text;
            string Pass = PasswordField.Text;

            try
            {
                 string Response = APIRequest.Request("login", new Dictionary<string, string>() {
                    {"username", Uname },
                    {"password", Pass }
                });

                //Parse Response
                Dictionary<string, string> ResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Response);
                string role = ResponseDict["role"];


                UserInfo user = new UserInfo
                {
                    Password = Pass,
                    Username = Uname,
                    Role = role,
                    Token = ResponseDict["key"]
                };
                string fileName = "data.json";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                string path = Path.Combine(documentsPath, fileName);

                File.WriteAllText(path, JsonConvert.SerializeObject(user));

                if (role == "student")
                {
                    Navigation.PushAsync(new Views.Student_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                }
                else if (role == "community")
                {
                    Navigation.PushAsync(new Views.Community_Member_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                }
                else if (role == "admin")
                {
                    Navigation.PushAsync(new Views.Administrator_Menu());
                    Xamarin.Forms.Application.Current.Properties["Token"] = ResponseDict["key"];
                    Xamarin.Forms.Application.Current.Properties["Role"] = role;
                }
                else
                {
                    FailedIndicator.Text = "Login Failed";
                }
            }
            catch(Exception ex)
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
                InsertsRow.Height = Insets.Top;
                string fileName = "topInsert.txt";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                string path = Path.Combine(documentsPath, fileName);

                File.WriteAllText(path, Insets.Top.ToString());

            }
        }
    }
}