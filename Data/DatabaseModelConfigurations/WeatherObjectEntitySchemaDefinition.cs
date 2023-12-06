using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.API.Models;

namespace Weather.API.Data.DatabaseModelConfigurations
{
    public class WeatherObjectEntitySchemaDefinition : IEntityTypeConfiguration<WeatherObject>
    {
        public void Configure(EntityTypeBuilder<WeatherObject> builder)
        {
            builder
                // .ToTable("Weathers", WeatherDbContext.DEFAULT_SCHEMA) //! Removed because SQ lite does not support schemas
                .ToTable(b => b.HasCheckConstraint("Weathers", "[Mintemperature] < [Maxtemperature]"));

            // Set a composite key using DateTime and Location
            builder.HasKey(w => new { w.Date, w.Location });

            // Configure properties
            builder.Property(w => w.Location)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(w => w.Mintemperature)
                .IsRequired();

            builder.Property(w => w.Maxtemperature)
                .IsRequired();

            builder.Property(w => w.Pressure)
                .IsRequired();

            builder.Property(w => w.AirQuality)
                .IsRequired();

            // Enum mapping
            builder
                .Property(w => w.AirQuality)
                .HasConversion(
                    v => v.ToString(),
                    v => (AirQuality)Enum.Parse(typeof(AirQuality), v)
                );


            // Other configurations as needed
        }
    }
}