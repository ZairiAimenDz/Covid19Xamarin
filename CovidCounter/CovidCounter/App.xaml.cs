using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidCounter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("Country"))
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new Introduction();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
