using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllCountries : ContentPage
    {
        private string url = "https://corona.lmao.ninja/v2/countries?yesterday&sort";
        private ObservableCollection<Class1> classes;
        public AllCountries()
        {
            InitializeComponent();
            Refresh();
        }
        async void Refresh()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Uri uri = new Uri(string.Format(url, string.Empty));
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<Class1[]>(content);
                        classes = new ObservableCollection<Class1>(items);
                        CountryList.ItemsSource = classes;
                        LoadingStuff.IsRunning = false;
                        LoadingStuff.IsVisible = false;
                    }
                }
            }
            catch
            {
                await DisplayAlert("Error", "Check Your Internet Connection", "ok");
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                CountryList.ItemsSource = classes;
            else
                CountryList.ItemsSource = classes.Where(ex => ex.country.ToLower().Contains(e.NewTextValue.ToLower()));
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            Refresh();
            ((RefreshView)sender).IsRefreshing = false;
        }
    }

    public class Class1
    {
        public long updated { get; set; }
        public string country { get; set; }
        public Countryinfo countryInfo { get; set; }
        public int cases { get; set; }
        public int todayCases { get; set; }
        public int deaths { get; set; }
        public int todayDeaths { get; set; }
        public int recovered { get; set; }
        public int todayRecovered { get; set; }
        public int active { get; set; }
        public int critical { get; set; }
        public int casesPerOneMillion { get; set; }
        public float deathsPerOneMillion { get; set; }
        public int tests { get; set; }
        public int testsPerOneMillion { get; set; }
        public int population { get; set; }
        public string continent { get; set; }
        public int oneCasePerPeople { get; set; }
        public int oneDeathPerPeople { get; set; }
        public int oneTestPerPeople { get; set; }
        public float activePerOneMillion { get; set; }
        public float recoveredPerOneMillion { get; set; }
        public float criticalPerOneMillion { get; set; }
    }

    public class Countryinfo
    {
        public int? _id { get; set; }
        public string iso2 { get; set; }
        public string iso3 { get; set; }
        public float lat { get; set; }
        public float _long { get; set; }
        public string flag { get; set; }
    }

}