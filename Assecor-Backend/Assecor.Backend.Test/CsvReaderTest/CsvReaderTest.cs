using Assecor.Backend.CsvAccess;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Mapping;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Test.CsvReaderTest
{
    public class CsvReaderTest
    {
        private readonly string _filesDirectory = "CsvReaderTest//TestFiles";
        private readonly ICsvReader<CsvPerson> _sut;
        private readonly ILogger<CsvReader<CsvPerson>> _loggerMock = Substitute.For<ILogger<CsvReader<CsvPerson>>>();

        public CsvReaderTest()
        {
            _sut = new CsvReader<CsvPerson>(_loggerMock);
        }

        private string GetCsvFilePath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, _filesDirectory, fileName);
        }

        [Fact]
        public async Task Should_Read_File()
        {
            var filePath = GetCsvFilePath("missing_lines.csv");
            var expectedAmount = 8;
            var result = await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);
            result.Count.ShouldBe(expectedAmount);
        }
    }
}
