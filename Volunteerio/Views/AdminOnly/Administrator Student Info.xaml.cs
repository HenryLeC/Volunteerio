using Microcharts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administrator_Student_Info : ContentPage
    {
        private Dictionary<string, string> StudentInfo { get; set; }
        private string UserGoal { get; set; } = "";
        private string UserGroup { get; set; } = "";

        public Administrator_Student_Info(Dictionary<string, string> Student)
        {
            InitializeComponent();

            StudentInfo = Student;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Dictionary<string, List<Dictionary<string, string>>> StudentHours = await APIRequest<Dictionary<string, List<Dictionary<string, string>>>>.RequestAsync("StudentHours", true, new Dictionary<string, string>()
                {
                    {"id", StudentInfo["ID"] }
                });

                StudentName.Text += StudentInfo["Name"];
                StudentId.Text += StudentInfo["StuId"];
                Hours.Text += StudentInfo["Hours"] + " of " + StudentInfo["HoursGoal"];

                foreach (var Hour in StudentHours["Hours"])
                {
                    TapGestureRecognizer click = new TapGestureRecognizer();
                    click.Tapped += (s, e) => {
                        HoursListView_ItemSelected(Hour);
                    };
                    StackLayout stack = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            new Label
                            {
                                Text = Hour["Reason"] + " | " + Hour["Confirmed"],
                                FontSize = 20.0
                            },
                            new Label
                            {
                                Text = Hour["Hours"],
                                FontSize = 20.0
                            }
                        }
                    };
                    stack.GestureRecognizers.Add(click);
                    PastHoursStack.Children.Add(stack);
                }

                foreach (var Hour in StudentHours["PastOpps"])
                {
                    PastOppsStack.Children.Add(new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            new Label
                            {
                                Text = Hour["Name"],
                                FontSize = 20.0
                            },
                            new Label
                            {
                                Text = Hour["Hours"],
                                FontSize = 20.0
                            }
                        }
                    });
                }

                if (StudentInfo["userSpecific"] == "true")
                {
                    UserGoalEntry.Text = StudentInfo["HoursGoal"];
                }

                var Groups = await APIRequest<List<Dictionary<string, string>>>.RequestAsync("getGroups", true, new Dictionary<string, string>());

                var GroupsClean = new List<string>() { 
                    "None"
                };

                foreach (var i in Groups)
                {
                    GroupsClean.Add(i["name"]);
                }

                GroupPicker.ItemsSource = GroupsClean;
                GroupPicker.SelectedItem = StudentInfo["group"];
            }
            catch
            {
                await DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Administrator_Menu());
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StudentInfo["Hours"] = (int.Parse(StudentInfo["Hours"]) + int.Parse((((Button)sender).CommandParameter as Dictionary<string, string>)["Hours"])).ToString();

                APIRequest.Request("confirmHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", (((Button)sender).CommandParameter as Dictionary<string,string>)["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIRequest.Request("deleteHours", true, new Dictionary<string, string>()
                {
                    { "StuHrData", (((Button)sender).CommandParameter as Dictionary<string,string>)["StuHrData"]}
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            Navigation.PushAsync(new Views.Administrator_Student_Info(StudentInfo));
        }

        private void HoursListView_ItemSelected(Dictionary<string, string> Hour)
        {
            Navigation.PushAsync(new ShowHours(Hour, StudentInfo));
        }

        private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Popup.IsVisible = true;
            await Popup.FadeTo(1, 200);
        }

        private async void ClosePopup_Clicked(object sender, EventArgs e)
        {
            await Popup.FadeTo(0, 200);
            Popup.IsVisible = false;

            try
            {
                if ((StudentInfo["userSpecific"] == "false" && UserGoalEntry.Text != "") || (StudentInfo["userSpecific"] == "true" && UserGoalEntry.Text != StudentInfo["HoursGoal"])){
                    string response = await APIRequest.RequestAsync("UserSpecificGoal", true, new Dictionary<string, string>
                    {
                        {"userId", StudentInfo["ID"] },
                        {"goal", UserGoal }
                    });

                    StudentInfo["HoursGoal"] = response;
                    Hours.Text = "No. of Hours: " + StudentInfo["Hours"] + " of " + StudentInfo["HoursGoal"];
                }

                if (UserGroup != "")
                {
                    string response = await APIRequest.RequestAsync("changeUserGroup", true, new Dictionary<string, string>
                    {
                        {"userId", StudentInfo["ID"] },
                        {"groupName", UserGroup }
                    });

                    StudentInfo["HoursGoal"] = response;
                    Hours.Text = "No. of Hours: " + StudentInfo["Hours"] + " of " + StudentInfo["HoursGoal"];
                }

            }
            catch
            {
                await DisplayAlert("Server Error", "Server Error, Please try again later", "Ok");
            }

        }

        private void UserGoalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserGoal = ((Entry)sender).Text;
        }

        private void GroupPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserGroup = GroupPicker.SelectedItem.ToString();
        }
    }

}