using Assecor.Backend.CsvAccess;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Exceptions;
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
            var act = async () =>  await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);

            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Should_Read_All_Items()
        {
            var expectedAmount = 11;
            var filePath = GetCsvFilePath("persons.csv");

            var persons = await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);

            persons.Count.ShouldBe(expectedAmount);
        }

        [Fact]
        public async Task Should_Skip_Empty_Lines()
        {
            var filePath = GetCsvFilePath("missing_lines.csv");
            var expectedAmount = 8;
            var result = await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);
            result.Count.ShouldBe(expectedAmount);
        }

        [Fact]
        public async Task Should_Log_Not_Parsable_Rows()
        {
            //file contains 3 lines, each will return null via mapping
            var filePath = GetCsvFilePath("notParsableLines.csv");
            Func<IEnumerable<string>, int, CsvPerson?> mappingFunc = (_, _) => null;

            await _sut.ReadFromCsvAsync(mappingFunc, filePath);
            
            var notParsableLinesAmount = 3;
            var expectedLogMessage = $"could not be parsed to object of type {nameof(CsvPerson)}, row is skipped";
            _loggerMock.Received(notParsableLinesAmount).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(expectedLogMessage)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Fact]
        public async Task Should_Log_Parsing_Exceptions()
        {
            //file contains not closed quotation marks in line 1, this should raise an exception
            var filePath = GetCsvFilePath("illFormattedLines.csv");
            var expectedError = "Csv reading error in row 1:";

            var act = async () => await _sut.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);

            var ex = await act.ShouldThrowAsync<CsvReaderException>();
            ex.Message.ShouldStartWith(expectedError);
        }
    }
}
