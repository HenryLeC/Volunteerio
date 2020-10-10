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
        private string userGoal { get; set; } = "";

        public Administrator_Student_Info(Dictionary<string, string> Student)
        {
            InitializeComponent();

            StudentInfo = Student;

            try
            {
                string response = APIRequest.Request("StudentHours", true, new Dictionary<string, string>()
                {
                    {"id", Student["ID"] }
                });
                Dictionary<string, List<Dictionary<string, string>>> StudentHours = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(response);

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

                //PastOppsListView.ItemsSource = StudentHours["PastOpps"];
                //HoursListView.ItemsSource = StudentHours["Hours"];

                //if (StudentHours["PastOpps"].Count == 0)
                //{
                //    PastOppsListView.HeightRequest = 50;
                //}
                //if (StudentHours["Hours"].Count == 0)
                //{
                //    HoursListView.HeightRequest = 50;
                //}
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
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

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Popup.IsVisible = true;
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            Popup.IsVisible = false;

            try
            {
                string response = APIRequest.Request("UserSpecificGoal", true, new Dictionary<string, string>
                {
                    {"userId", StudentInfo["ID"] },
                    {"goal", userGoal }
                });
            }
            catch
            {
                DisplayAlert("Server Error", "Server Error, Please try again later", "Ok");
            }

        }

        private void UserGoalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            userGoal = ((Entry)sender).Text;
        }

    }

}