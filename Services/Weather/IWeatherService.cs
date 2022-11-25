using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using ViewModel;

namespace Services.Weather
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherViewModel>> GetWeatherByCity(string cityName);
        Task<IEnumerable<WeatherViewModel>> GetWeatherByZipCode(string zipCode);
        Task<Result<bool>> SaveWeather(List<WeatherViewModel> model);
        Task<IEnumerable<WeatherViewModel>> GetWeatherHistory();
    }
}