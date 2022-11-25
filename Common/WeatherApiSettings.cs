using System.ComponentModel.DataAnnotations;
using Common.Enums;


namespace Common
{
    public class WeatherApiSettings
    {
        public const string Key = "WeatherApi";

        [Required]
        public string Url { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public UnitsType Units { get; set; }
    }
}
