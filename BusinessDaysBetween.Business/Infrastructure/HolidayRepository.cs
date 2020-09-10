using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<HolidayDto>> LoadHolidays()
        {
            string holidaysRaw;
            try
            {
                holidaysRaw = await _fileSystem.File.ReadAllTextAsync("holidays.json");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load holiday data!");
                return Enumerable.Empty<HolidayDto>();
            }
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());

            List<HolidayDto> holidayDtos;
            try
            {
                holidayDtos = JsonSerializer.Deserialize<List<HolidayDto>>(holidaysRaw, options);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Failed to deserialise holiday data!");
                return Enumerable.Empty<HolidayDto>();
            }
            
            var validator = new HolidayDtoValidator();
            
            // ensure we call validate only once per DTO by connecting up object and validation result
            var validationResults = holidayDtos.Select(h => new {Dto = h, ValidationResult = validator.Validate(h)}).ToList();
            
            // report errors
            foreach (var failure in validationResults.Select((v, i) => new { Value = v.ValidationResult, Index = i}))
            {
                if (!failure.Value.IsValid)
                {
                    _logger.LogError($"Validation failed for holiday at index {failure.Index}: {string.Join(',', failure.Value.Errors)}");
                }
            }

            // return only validated DTOs
            return validationResults.Where(v => v.ValidationResult.IsValid).Select(v => v.Dto);
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