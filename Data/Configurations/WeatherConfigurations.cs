using Data.Configurations.CoreConfigurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class WeatherConfigurations : BaseEntityTypeConfiguration<Weather>
    {
        public override void Configure(EntityTypeBuilder<Weather> entityTypeBuilder)
        {
            ConfigureWeatherSchema(entityTypeBuilder);
            base.Configure(entityTypeBuilder);
        }

        private static void ConfigureWeatherSchema(EntityTypeBuilder<Weather> builder)
        {
            builder.Property(e => e.Date)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Temperature)
                .IsRequired();

            builder.Property(e => e.Humidity)
                .IsRequired();

        }
    }
}
