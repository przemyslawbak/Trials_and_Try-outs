using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XamData.Model;

namespace XamData.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private string station;

        public string StationName
        {
            get { return station; }
            set
            {
                station = value;
                NotifyPropertyChanged();
            }
        }

        private long el;

        public long Elevation
        {
            get { return el; }
            set
            {
                el = value;
                NotifyPropertyChanged();
            }
        }

        private long temp;

        public long Temperature
        {
            get { return temp; }
            set
            {
                temp = value;
                NotifyPropertyChanged();
            }
        }

        private long humid;

        public long Humidity
        {
            get { return humid; }
            set
            {
                humid = value;
                NotifyPropertyChanged();
            }
        }

        public async Task GetWeatherAsync(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = await client.GetAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode();
            var JsonResult = response.Content.ReadAsStringAsync().Result;
            var weather =
                JsonConvert.DeserializeObject<WeatherResult>(JsonResult);
            SetValues(weather);
        }

        private void SetValues(WeatherResult weather)
        {
            var stationName = weather.weatherObservation.stationName;
            var elevation = weather.weatherObservation.elevation;
            var temperature = weather.weatherObservation.temperature;
            var humidity = weather.weatherObservation.humidity;

            StationName = stationName;
            Elevation = elevation;
            Temperature = temperature;
            Humidity = humidity;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
