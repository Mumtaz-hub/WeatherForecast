using System;
using Mapster;
using ViewModel;

namespace Services.MapsterRegistration
{
    public class WeatherMapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<WeatherViewModel, Domain.Entities.Weather>()
                .Map(dest => dest.Temperature, opts => opts.AverageTemperature)
                .Map(dest => dest.Humidity, opts => opts.AverageHumidity)
                .Map(dest => dest.CreationTs, opts => DateTime.UtcNow);

            config.NewConfig<Domain.Entities.Weather, WeatherViewModel>()
                .Map(dest => dest.AverageTemperature, opts => opts.Temperature)
                .Map(dest => dest.AverageHumidity, opts => opts.Humidity);

        }
    }
}
