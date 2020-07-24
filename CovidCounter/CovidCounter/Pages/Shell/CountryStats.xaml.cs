using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountryStats : ContentPage
    {
        private CountryTotal ct = new CountryTotal();
        private List<ChartEntry> entries;
        public CountryStats()
        {
            InitializeComponent();
            Refresh();
        }
        async void  Refresh()
        {
            try
            {
                string url = "https://covid19-api.org/api/status/" + Application.Current.Properties["Country"];
                using (var client = new HttpClient())
                {
                    Uri uri = new Uri(string.Format(url, string.Empty));
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        ct = JsonConvert.DeserializeObject<CountryTotal>(content);
                        BindingContext = ct;
                    }
                }
                url = "https://covid19-api.org/api/timeline/" + Application.Current.Properties["Country"];
                using (var client = new HttpClient())
                {
                    Uri uri = new Uri(string.Format(url, string.Empty));
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        entries = new List<ChartEntry>();
                        string content = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<CountryTotal[]>(content);
                        var realit = items.Take(7);
                        newdeath.Text = (realit.ElementAt(0).deaths - realit.ElementAt(1).deaths).ToString("N0");
                        newinfec.Text = (realit.ElementAt(0).cases - realit.ElementAt(1).cases).ToString("N0");
                        newrec.Text = (realit.ElementAt(0).recovered - realit.ElementAt(1).recovered).ToString("N0");
                        foreach (var item in realit.Reverse())
                        {
                            entries.Add(new ChartEntry(item.cases) { Label = item.last_update.ToString("MMM dd"), ValueLabel = item.cases.ToString(), Color = SKColor.Parse("#FB4D4F") });
                        }
                    }
                }
                TotalCases.Chart = new LineChart() { Entries = entries,LineMode=LineMode.Straight, LabelTextSize = 32 ,MinValue= (float)(entries.ElementAt(0).Value- entries.ElementAt(0).Value*0.1) };
            }
            catch
            {
                await DisplayAlert("Error", "Check Your Internet Connection" + Application.Current.Properties["Country"], "ok");
            }
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            Refresh();
            ((RefreshView)sender).IsRefreshing = false;
        }
    }

    public class CountryTotal
    {
        public string country { get; set; }
        public DateTime last_update { get; set; }
        public int cases { get; set; }
        public int deaths { get; set; }
        public int recovered { get; set; }
    }

}