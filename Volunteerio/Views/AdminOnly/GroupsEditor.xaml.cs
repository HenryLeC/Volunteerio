using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Views.AdminOnly
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsEditor : ContentPage
    {
        private string GroupName = "";
        private string GroupGoal = "";
        private readonly Dictionary<string, string> GroupNameToId = new Dictionary<string, string>();

        public GroupsEditor()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var data = await APIRequest<List<Dictionary<string, string>>>.RequestAsync("getGroups", true, new Dictionary<string, string>());

            BindableLayout.SetItemsSource(GroupsStack, data);

            GroupNameToId.Clear();

            foreach (var i in data)
            {
                GroupNameToId.Add(i["name"], i["id"]);
                RemoveGroupPicker.Items.Add(i["name"]);
            }

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

        private async void ConfirmDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                string res = APIRequest.Request("deleteGroup", true, new Dictionary<string, string>
                {
                    {"groupId", GroupNameToId[RemoveGroupPicker.SelectedItem as string] }
                });

                OnAppearing();
            }
            catch
            {
                await DisplayAlert("Server Error", "Server Error, Server Error, Please try again later", "Ok");
            }

            await RemovePopup.FadeTo(0, 200);
            RemovePopup.IsVisible = false;
        }

        private async void DenyDelete_Clicked(object sender, EventArgs e)
        {
            await RemovePopup.FadeTo(0, 200);
            RemovePopup.IsVisible = false;
        }

        private async void RemButton_Clicked(object sender, EventArgs e)
        {
            RemovePopup.IsVisible = true;
            await RemovePopup.FadeTo(1, 200);
        }
    }
}