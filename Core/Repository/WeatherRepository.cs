using Core.Interfaces;
using Core.Persistence;
using Data;
using Domain.Entities;

namespace Core.Repository
{
    public class WeatherRepository : GenericRepository<Weather>, IWeatherRepository
    {
        public WeatherRepository(DatabaseContext context) : base(context)
        {
            
        }
    }
}
