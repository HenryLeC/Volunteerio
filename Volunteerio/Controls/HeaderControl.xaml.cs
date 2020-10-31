using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volunteerio.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderControl : Grid
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Grid));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public HeaderControl()
        {
            InitializeComponent();

            string fileName = "topInsert.txt";
            string documentsPath = Xamarin.Forms.Application.Current.Properties["docsPath"].ToString(); // Documents folder
            string path = Path.Combine(documentsPath, fileName);
            if (File.Exists(path))
            {
                InsertsRow.Height = Double.Parse(File.ReadAllText(path));
            }
            else
            {
                InsertsRow.Height = 0;
            }
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            //HamburgerButtonClicked(this, null);
            Navigation.PopToRootAsync();
        }

    }
}