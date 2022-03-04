using ForecastApp.OpenWeatherMapModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ForecastApp.Repositories
{
    public interface IForecastRepository
    {        
        Task<WeatherResponse> GetForecastASync(string city);
      
    }
}