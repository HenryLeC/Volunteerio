using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views.AdminOnly
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsEditor : ContentPage
    {
        private string GroupName = "";
        private string GroupGoal = "";

        public GroupsEditor()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var data = await APIRequest<List<Dictionary<string, string>>>.RequestAsync("getGroups", true, new Dictionary<string, string>());

            BindableLayout.SetItemsSource(GroupsStack, data);
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            AddPopup.IsVisible = true;
            await AddPopup.FadeTo(1, 200);
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            await AddPopup.FadeTo(0, 200);
            AddPopup.IsVisible = false;

            try
            {
                string res = APIRequest.Request("makeGroup", true, new Dictionary<string, string>
                {
                    {"hoursGoal", GroupGoal },
                    {"name", GroupName }
                });

                OnAppearing();
            }
            catch
            {
                await DisplayAlert("Server Error", "Server Error, Please Try Again Later", "Ok");
            }
            
        }

        private void GroupNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupName = ((Entry)sender).Text;
        }

        private void GroupGoalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupGoal = ((Entry)sender).Text;
        }

        private void ClickGroup_Clicked(object sender, EventArgs e)
        {
            TapGestureRecognizer senderTap = (TapGestureRecognizer)sender;
            Dictionary<string, string> data = senderTap.CommandParameter as Dictionary<string, string>;
        }
    }
}