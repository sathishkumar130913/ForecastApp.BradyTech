using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastApp.Config;
using ForecastApp.OpenWeatherMapModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;

namespace ForecastApp.Repositories
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly HttpClient _httpClient  =new HttpClient();
        public ForecastRepository(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
         
        public ForecastRepository()
        {

        }

        public async Task<WeatherResponse> GetForecastASync(string city)
        {
            
            string IDOWeather = Constants.OPEN_WEATHER_APPID;

            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", city, IDOWeather);

           // var client  = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get,url);

            var response = await _httpClient.SendAsync(request);
           
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var resultcontent = await response.Content.ReadAsStringAsync();
                    dynamic WeatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(resultcontent);
                    return WeatherResponse;
                }            

            return null;
        }

       
    }
}
