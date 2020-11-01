using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volunteerio.Views.AdminOnly;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class School_Settings : ContentPage
    {
        private readonly List<List<View>> users = new List<List<View>>();
        public School_Settings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string resp = APIRequest.Request("schoolSettings", true, new Dictionary<string, string>());

            Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp);

            HoursGoal.Text = settings["SGoal"];

            #region User Add Grid
            Grid StudentInput = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Star},
                }
            };
            Entry name = new Entry { Placeholder = "Name" };
            StudentInput.Children.Add(name, 0, 0);
            Entry UName = new Entry { Placeholder = "User Name" };
            StudentInput.Children.Add(UName, 1, 0);
            Entry passWd = new Entry { Placeholder = "Password" };
            StudentInput.Children.Add(passWd, 2, 0);
            Entry iD = new Entry { Placeholder = "Id" };
            StudentInput.Children.Add(iD, 0, 1);
            Picker role = new Picker { Title = "Role" };
            role.Items.Add("Student");
            role.Items.Add("Admin");
            role.Items.Add("Teacher");
            role.Items.Add("Community Member");
            role.SelectedIndex = 0;
            StudentInput.Children.Add(role, 1, 1);
            Button add = new Button { Text = "Add" };
            add.CommandParameter = new List<View> { name, UName, passWd, iD, role, add };
            add.Clicked += Add_Clicked;
            StudentInput.Children.Add(add, 2, 1);
            UsersStack.Children.Clear();
            UsersStack.Children.Add(StudentInput);
            #endregion
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            Button senderB = (Button)sender;
            Grid grid = (Grid)senderB.Parent;
            List<View> items = new List<View>();
            items = senderB.CommandParameter as List<View>;
            int itemsI = 0;
            foreach (View item in items)
            {
                if (itemsI >= 4) { }
                else if (((Entry)item).Text == "" || ((Entry)item).Text == null)
                {
                    return;
                }
                itemsI++;
            }
            users.Add(senderB.CommandParameter as List<View>);
            grid.Children.Remove(items.Last());
            #region User Add Grid
            Grid StudentInput = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Star},
                }
            };
            Entry name = new Entry { Placeholder = "Name" };
            StudentInput.Children.Add(name, 0, 0);
            Entry UName = new Entry { Placeholder = "User Name" };
            StudentInput.Children.Add(UName, 1, 0);
            Entry passWd = new Entry { Placeholder = "Password" };
            StudentInput.Children.Add(passWd, 2, 0);
            Entry iD = new Entry { Placeholder = "Id" };
            StudentInput.Children.Add(iD, 0, 1);
            Picker role = new Picker { Title = "Role" };
            role.Items.Add("Student");
            role.Items.Add("Admin");
            role.Items.Add("Teacher");
            role.Items.Add("Community Member");
            role.SelectedIndex = 0;
            StudentInput.Children.Add(role, 1, 1);
            Button add = new Button { Text = "Add" };
            add.CommandParameter = new List<View> { name, UName, passWd, iD, role, add };
            add.Clicked += Add_Clicked;
            StudentInput.Children.Add(add, 2, 1);
            UsersStack.Children.Add(StudentInput);
            #endregion
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            HoursGoal.IsReadOnly = !HoursGoal.IsReadOnly;
            if (HoursGoal.IsReadOnly)
            {
                HoursGoal.BackgroundColor = Color.Gray;
            }
            else
            {
                HoursGoal.BackgroundColor = Color.Default;
            }

            try
            {
                APIRequest.Request("schoolGoal", true, new Dictionary<string, string> {
                    {"Goal", HoursGoal.Text }
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private void HoursGoal_Completed(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("schoolGoal", true, new Dictionary<string, string> {
                    {"Goal", ((Entry)sender).Text }
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private void SubmitstudentsButton_Clicked(object sender, EventArgs e)
        {
            int attr;
            int usersI = 0;
            Dictionary<String, String> usersDict = new Dictionary<string, string>();
            List<string> TypeOrder = new List<string> { "N", "UN", "P", "I" };

            foreach (List<View> user in users)
            {
                attr = 0;
                foreach (View att in user)
                {
                    if (attr != 5)
                    {
                        if (attr == 4)
                        {
                            usersDict.Add("user" + usersI.ToString() + "R", ((Picker)att).SelectedItem as string);
                        }
                        else
                        {
                            usersDict.Add("user" + usersI.ToString() + TypeOrder[attr], ((Entry)att).Text);
                        }
                    }
                    attr++;
                }
                usersI++;
            }

            usersDict.Add("users", usersI.ToString());

            try
            {
                string response = APIRequest.Request("addUsers", true, usersDict);
                Navigation.PopToRootAsync();
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
        }

        private async void ReportButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                aiStack.IsVisible = true;
                ai.IsRunning = true;

                string path = "";

                await Task.Run(() =>
                {
                    //Create Client and Request
                    var client = new RestClient(APIRequest.server + "StudentReport")
                    {
                        Timeout = 5000
                    };
                    var request = new RestRequest(Method.POST)
                    {
                        AlwaysMultipartFormData = true
                    };
                    request.AddParameter("x-access-token", Application.Current.Properties["Token"] as string);

                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception();
                    }

                    string time = DateTime.Now.ToString("MMddyyHHmm");
                    path = Path.Combine(Application.Current.Properties["docsPath"] as string, "StudentReport" + time + ".csv");

                    File.WriteAllBytes(path, response.RawBytes);
                });

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(path, "text/csv")
                });

                aiStack.IsVisible = false;
                ai.IsRunning = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                aiStack.IsVisible = false;
                ai.IsRunning = false;
                await DisplayAlert("Error", "Server Error, Please Try Again Later", "OK");
            }
        }

        private void EditGroupsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GroupsEditor());
        }
    }
}