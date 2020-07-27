﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Forms.PlatformConfiguration;

namespace Volunteerio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderControl : Grid
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Grid));

        public delegate void HamburgerButtonHandler(object source, EventArgs e);
        public event HamburgerButtonHandler HamburgerButtonClicked;

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

            InsertsRow.Height = Double.Parse(File.ReadAllText(path));
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            HamburgerButtonClicked(this, null);
        }

    }
}