using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Weather;
using ViewModel;

namespace Api.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
 
        private readonly IWeatherService weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        [HttpGet]
        [Route("api/weather/forecastbycity")]
        public async Task<IEnumerable<WeatherViewModel>> GetWeatherByCity(string cityName)
        {
            return await weatherService.GetWeatherByCity(cityName);
        }


        [HttpGet]
        [Route("api/weather/forecastbyzipcode")]
        public async Task<IEnumerable<WeatherViewModel>> GetWeatherByZipCode(string zipCode)
        {
            return await weatherService.GetWeatherByZipCode(zipCode);
        }


        [HttpGet]
        [Route("api/weatherhistory")]
        public async Task<IEnumerable<WeatherViewModel>> GetWeatherHistory()
        {
            return await weatherService.GetWeatherHistory();
        }
    }
}