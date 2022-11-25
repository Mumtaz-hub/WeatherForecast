using System;
using Domain.CoreEntities;

namespace Domain.Entities
{
    public class Weather : BaseEntity
    {
        public string Date { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
    }
}
