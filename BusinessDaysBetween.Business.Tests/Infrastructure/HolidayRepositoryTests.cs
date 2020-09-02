using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.Tests.TestHelpers;
using BusinessDaysBetween.Business.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BusinessDaysBetween.Business.Tests.Infrastructure
{
    public class HolidayRepositoryTests
    {
        [Fact]
        public async Task LoadHolidays_ReturnsHolidays_WithValidData()
        {
            // arrange
            var rawJson = ResourceHelpers.ReadEmbeddedResource("BusinessDaysBetween.Business.Tests.TestResources.holidays_valid.json");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "holidays.json", new MockFileData(rawJson) },
            });
            var sut = new HolidayRepository(fileSystem, NullLogger<HolidayRepository>.Instance);

            // act
            var result = (await sut.LoadHolidays()).ToList();
            
            // assert
            Assert.Equal(3, result.Count());
            Assert.Collection(result, 
                h => Assert.Equal(HolidayType.Fixed, h.Type),
                h => Assert.Equal(HolidayType.RollsToMonday, h.Type),
                h => Assert.Equal(HolidayType.ParticularDayOfMonth, h.Type));
            
            // for now I'm satisfied, maybe more assertions...
        }
    }
}