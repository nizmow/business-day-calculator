using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BusinessDaysBetween.Business.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BusinessDaysBetween.Business.Infrastructure
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<HolidayRepository> _logger;

        public HolidayRepository(IFileSystem fileSystem, ILogger<HolidayRepository> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public async Task<IEnumerable<Holiday>> LoadHolidays()
        {
            string holidaysRaw;
            try
            {
                holidaysRaw = await _fileSystem.File.ReadAllTextAsync("holidays.json");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load holiday data!");
                return Enumerable.Empty<Holiday>();
            }
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            try
            {
                var holidays = JsonSerializer.Deserialize<Holiday[]>(holidaysRaw, options);
                return holidays.AsEnumerable();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Failed to deserialise holiday data!");
                return Enumerable.Empty<Holiday>();
            }
        }

        /// <summary>
        /// Let's give the user the ability to enter nicer dates without worrying too much about iso8601
        /// </summary>
        private class DateTimeConverterUsingDateTimeParse : System.Text.Json.Serialization.JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), "yyyy-M-d", null);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                // don't use this to write things... cut down on feature creep...
                throw new NotImplementedException();
            }
        }
    }
}