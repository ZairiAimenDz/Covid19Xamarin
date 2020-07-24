using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CovidCounter
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : Shell
    {
        /*
            API Links :
            - Difference Depending On The Country : https://covid19-api.org/api/diff/:country /with :country = Country Code
            - Total Cases For Every Country : https://covid19-api.org/api/status
            - Get Cases By Date : https://covid19-api.org/api/status?date=2020-03-25
            - Get Cases By Date And Country : https://covid19-api.org/api/status/:country?date=2020-03-25
            - World Cases Timeline : https://covid19-api.org/api/timeline
            - Timeline By Country : https://covid19-api.org/api/timeline/:country
            - Return A List Of COuntries : https://covid19-api.org/api/countries
            - Global Stats : https://api.covid19api.com/stats

         */


        public MainPage()
        {
            InitializeComponent();
        }
    }
}
