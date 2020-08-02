using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Microcharts;

namespace Volunteerio.Views
{
    public partial class Student_Dashboard : ContentPage
    {
        public Student_Dashboard()
        {
            InitializeComponent();

            try
            {
                string response = APIRequest.Request("hours", true, new Dictionary<string, string>());

                Dictionary<string, string> HoursDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                //HoursText.Text = 
                int Goal = int.Parse(HoursDict["goal"]);
                int Current = int.Parse(HoursDict["hours"]);
                int GoalP;
                if (Goal - Current < 0)
                {
                    GoalP = 0;
                }
                else
                {
                    GoalP = Goal - Current;
                }
                List<ChartEntry> entries = new List<ChartEntry>
                {
                    new ChartEntry(GoalP)
                    {
                        Label = "Goal",
                        ValueLabel = Goal.ToString(),
                        Color = SkiaSharp.SKColor.FromHsv(69, 69, 69),
                    },
                    new ChartEntry(Current)
                    {
                        Label = "Current",
                        ValueLabel = Current.ToString(),
                        Color = SkiaSharp.SKColor.FromHsv(69, 69, 420)
                    }
                    
                };
                Chart.Chart = new PieChart()
                {
                    Entries = entries,
                    LabelTextSize = 50,
                    GraphPosition = GraphPosition.Center
                };
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }


        }

        private void AddHoursButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (HoursEntry.Text != "" && ReasonEntry.Text != "" && Int32.TryParse(HoursEntry.Text, out int HoursInt))
                {

                    APIRequest.Request("addhours", true, new Dictionary<string, string>()
                    {
                        {"hours", HoursEntry.Text },
                        {"reason", ReasonEntry.Text }
                    });

                    HoursEntry.Text = "";
                    ReasonEntry.Text = "";

                    DisplayAlert("Hours Added", "Your Hours Have Been Added, Please Wait For Them To Be Confirmed", "Ok");
                }
                else
                {
                    DisplayAlert("Invalid", "Please Enter a Number For Hours And Text For Reason", "Ok");
                }

            }
            catch (ServerErrorException)
            {

                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }

        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Student_Menu());
        }

        private void MarketplaceButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Volunteerio.Views.Marketplace_Page());
        }
    }
}
