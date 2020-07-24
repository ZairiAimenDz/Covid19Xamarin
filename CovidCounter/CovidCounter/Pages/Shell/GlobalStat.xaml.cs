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
    public partial class GlobalStat : ContentPage
    {
        private GlobalStats ct = new GlobalStats();
        private List<ChartEntry> entries;
        public GlobalStat()
        {
            InitializeComponent();
            Refresh();
        }
        async void Refresh()
        {
            try
            {
                string url = "https://covid19-api.org/api/timeline";
                using (var client = new HttpClient())
                {
                    Uri uri = new Uri(string.Format(url, string.Empty));
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        entries = new List<ChartEntry>();
                        string content = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<GlobalStats[]>(content);
                        var realit = items.Take(14);
                        ct = realit.First();
                        BindingContext = ct;
                        newdeath.Text = (realit.ElementAt(0).total_deaths - realit.ElementAt(1).total_deaths).ToString("N0");
                        newinfec.Text = (realit.ElementAt(0).total_cases - realit.ElementAt(1).total_cases).ToString("N0");
                        newrec.Text = (realit.ElementAt(0).total_recovered - realit.ElementAt(1).total_recovered).ToString("N0");
                        foreach (var item in realit.Reverse())
                        {
                            entries.Add(new ChartEntry(item.total_cases) { Label = item.last_update.ToString("MMM dd"), ValueLabel = item.total_cases.ToString(), Color = SKColor.Parse("#FB4D4F") });
                        }
                    }
                }
                TotalCases.Chart = new LineChart() { Entries = entries,LineMode=LineMode.Straight,LabelTextSize = 32 ,MinValue=entries.ElementAt(0).Value-1000000};
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

    public class GlobalStats
    {
        public DateTime last_update { get; set; }
        public int total_cases { get; set; }
        public int total_deaths { get; set; }
        public int total_recovered { get; set; }
    }


}