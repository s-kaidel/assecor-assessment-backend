using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Mapping;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.Test
{
    public class CsvReaderTest
    {
        private readonly CsvOptions _options = new()
        {
            FileName = "test.csv"
        };
        private readonly ICsvReader<CsvPerson> _sut;
        private readonly IOptions<CsvOptions> _optionsMock = Substitute.For<IOptions<CsvOptions>>();
        private readonly ILogger<CsvReader<CsvPerson>> _loggerMock = Substitute.For<ILogger<CsvReader<CsvPerson>>>();

        public CsvReaderTest()
        {
            _optionsMock.Value.Returns(_options);
            _sut = new CsvReader<CsvPerson>(_loggerMock, _optionsMock);
        }

        [Fact]
        public async Task Should_Read_File()
        {
            var result = await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow);
        }
    }
}
