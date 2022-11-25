using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Common.Constants;
using Common.Extensions;
using Core.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Options;
using ViewModel;

namespace Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApiSettings weatherApiSettings;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public WeatherService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<WeatherApiSettings> weatherApiSettings, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.weatherApiSettings = weatherApiSettings.Value;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<WeatherViewModel>> GetWeatherByCity(string cityName)
        {
            string url = $"{weatherApiSettings.Url}?APPID={weatherApiSettings.ApiKey}&q={cityName}&units={weatherApiSettings.Units}";
            return await GetWeatherFromApi(url);
        }

        public async Task<IEnumerable<WeatherViewModel>> GetWeatherByZipCode(string zipCode)
        {
            string url = $"{weatherApiSettings.Url}?APPID={weatherApiSettings.ApiKey}&q={zipCode}&units={weatherApiSettings.Units}";
            return await GetWeatherFromApi(url);
        }

        public async Task<Result<bool>> SaveWeather(List<WeatherViewModel> model)
        {
            var entityList = model.Select(mapper.Map<Domain.Entities.Weather>).ToList();
            await unitOfWork.WeatherRepository.AddRange(entityList);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransaction();
            return new SuccessResult<bool>(true);
        }

        public async Task<IEnumerable<WeatherViewModel>> GetWeatherHistory()
        {
            var data = await unitOfWork.WeatherRepository.GetAll(orderBy: i => i.OrderByDescending(k => k.Id));
            return data.Select(mapper.Map<WeatherViewModel>).ToList();
        }
 
        private async Task<WeatherResponseViewModel> ExecuteOpenWeatherMapApi(string url)
        {
            using var client = httpClientFactory.CreateClient();
            var jsonData = await client.GetStringAsync(url);
            var result = jsonData.ToDeserialize<WeatherResponseViewModel>();
            return result;
        }

        private List<WeatherViewModel> GetWeatherData(WeatherResponseViewModel result)
        {
            var data = result?.list
                .GroupBy(o => new { EventDate = o.Date.Date })
                .Select(s => new WeatherViewModel
                {
                    Date = s.Key.EventDate.ToString(ApplicationConstants.DefaultDateFormat, CultureInfo.CurrentCulture),
                    City = result.city?.name,
                    Country = result.city?.country,
                    AverageTemperature = $"{Math.Round(s.Average(o => o.main.temp), 2, MidpointRounding.ToEven)} {weatherApiSettings.Units.ToEnumDescription()}",
                    AverageHumidity = $"{Math.Round(s.Average(o => o.main.humidity), 2, MidpointRounding.ToEven)} %"
                })
                .OrderBy(o => o.Date)
                .ToList();
            return data;
        }

        private async Task<IEnumerable<WeatherViewModel>> GetWeatherFromApi(string url)
        {
            var result = await ExecuteOpenWeatherMapApi(url);
            var data = GetWeatherData(result);
            if (data != null && data.Any())
                await SaveWeather(data);

            return data;
        }
    }
}