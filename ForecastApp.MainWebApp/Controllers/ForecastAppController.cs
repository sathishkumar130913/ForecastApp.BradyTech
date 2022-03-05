using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastApp.ForecastAppModels;
using ForecastApp.OpenWeatherMapModels;
using ForecastApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForecastApp.Controllers
{
    //[Authorize]
    public class ForecastAppController : Controller
    {
        private readonly IForecastRepository _forecastRepository;

        // Dependency Injection
        public ForecastAppController(IForecastRepository forecastAppRepo)
        {
            _forecastRepository = forecastAppRepo;
        }

        // GET: ForecastApp/SearchCity
        public IActionResult SearchCity()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");

                var viewModel = new SearchCity();
                return View(viewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // POST: ForecastApp/SearchCity
        [HttpPost]
        public IActionResult SearchCity(SearchCity model)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");

                // If the model is valid, consume the Weather API to bring the data of the city
                if (ModelState.IsValid)
                {
                    return RedirectToAction("City", "ForecastApp", new { city = model.CityName });
                }
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: ForecastApp/City
        public async Task<IActionResult> City(string city)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
                // Consume the OpenWeatherAPI in order to bring Forecast data in our page.
                WeatherResponse weatherResponse = await _forecastRepository.GetForecastASync(city);
                City viewModel = new City();

                if (weatherResponse != null)
                {
                    viewModel.Name = weatherResponse.Name;
                    viewModel.Humidity = weatherResponse.Main.Humidity;
                    viewModel.Pressure = weatherResponse.Main.Pressure;
                    viewModel.Temp = weatherResponse.Main.Temp;
                    viewModel.Weather = weatherResponse.Weather[0].Main;
                    viewModel.Icon = "http://openweathermap.org/img/w/" + weatherResponse.Weather[0].Icon + ".png";
                    viewModel.Wind = weatherResponse.Wind.Speed;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
    }
}