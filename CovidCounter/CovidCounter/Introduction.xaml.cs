using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Introduction : ContentPage
    {
        public Introduction()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                string url = "https://covid19-api.org/api/countries";
                using (var client = new HttpClient())
                {
                    Uri uri = new Uri(string.Format(url, string.Empty));
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<Class1[]>(content);
                        Countries.ItemsSource = items;
                        Countries.IsVisible = true;
                    }
                }
            }
            catch
            {
                await DisplayAlert("Error", "Check Your Internet Connection", "ok");
            }
            LoadingStuff.IsVisible = LoadingStuff.IsRunning = false;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //https://covid19-api.org/api/countries
            if (Countries.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please Select A Country From The List", "ok");
            }
            else
            {
                Application.Current.Properties.Add("Country", (Countries.SelectedItem as Class1).alpha2);
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainPage();
            }

        }
    }

    class Class1
    {
        public string name { get; set; }
        public string alpha2 { get; set; }
        public string alpha3 { get; set; }
        public string numeric { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }

        public override string ToString()
        {
            return name + "  " + alpha2;
        }
    }

}