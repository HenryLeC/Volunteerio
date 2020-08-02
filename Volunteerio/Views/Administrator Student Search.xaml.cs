using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Volunteerio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administrator_Student_Search : ContentPage
    {
        public Administrator_Student_Search()
        {
            InitializeComponent();
        }

        private void StudentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Student_Info(e.SelectedItem as Dictionary<string, string>));
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrator_Menu());
        }

        private void StudentSearchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string response = APIRequest.Request("StudentsList", true, new Dictionary<string, string>() {
                    {"Filter", FilterField.Text }
                });
                List<Dictionary<string, string>> students = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);

                StudentsListView.ItemsSource = students;
            }
            catch
            {
                DisplayAlert("Server Error", "Please Try Agin Later", "OK");
            }
            
        }
    }
}