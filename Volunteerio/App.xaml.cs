using System;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Volunteerio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.Login());
        }


        protected override void OnStart()
        {
            string fileName = "data.json";
            string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
            string path = Path.Combine(documentsPath, fileName);

            Console.WriteLine(path);

            if (File.Exists(path))
            {
                Dictionary<string, string> userInfoDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

                string Uname = userInfoDict["Username"];
                string Pass = userInfoDict["Password"];

                try
                {
                    string Response = APIRequest.Request("login", new Dictionary<string, string>() {
                        {"username", Uname },
                        {"password", Pass }
                    });

                    //Parse Response
                    Dictionary<string, string> ResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Response);
                    string role = ResponseDict["role"];

                    if (role == "student")
                    {
                        MainPage = new NavigationPage(new Views.Student_Menu());
                        Current.Properties["Token"] = ResponseDict["key"];
                        Current.Properties["Role"] = role;
                    }
                    else if (role == "community")
                    {
                        MainPage = new NavigationPage(new Views.Community_Member_Menu());
                        Current.Properties["Token"] = ResponseDict["key"];
                        Current.Properties["Role"] = role;
                    }
                    else if (role == "admin")
                    {
                        MainPage = new NavigationPage(new Views.Administrator_Menu());
                        Current.Properties["Token"] = ResponseDict["key"];
                        Current.Properties["Role"] = role;
                    }
                }
                catch (Exception)
                {
                    MainPage = new NavigationPage(new Views.Login());
                }

            }

            else
            {
                MainPage = new NavigationPage(new Views.Login());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            string fileName = "data.json";
            string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
            string path = Path.Combine(documentsPath, fileName);

            if (File.Exists(path))
            {
                Dictionary<string, string> userInfoDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

                string Uname = userInfoDict["Username"];
                string Pass = userInfoDict["Password"];

                try
                {
                    string Response = APIRequest.Request("login", new Dictionary<string, string>() {
                        {"username", Uname },
                        {"password", Pass }
                    });

                    //Parse Response
                    Dictionary<string, string> ResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Response);
                    Current.Properties["Token"] = ResponseDict["key"];
                }
                catch
                {
                    MainPage = new NavigationPage(new Views.Login());
                }
            }
        }
    }
}
