using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Dal.Provider;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.Test.ProviderTests
{
    public class CsvPersonProviderTest
    {
        private readonly ICsvPersonProvider _sut;
        private readonly ICsvReader<CsvPerson> _readerMock = Substitute.For<ICsvReader<CsvPerson>>();
        private readonly ILogger<CsvPersonProvider> _loggerMock = Substitute.For<ILogger<CsvPersonProvider>>();
        private CsvSettings _settings = new();
        private readonly IOptions<CsvSettings> _settingsMock = Substitute.For<IOptions<CsvSettings>>();
        private readonly ICsvFileLocationHandler _fileHandlerMock = Substitute.For<ICsvFileLocationHandler>();

        public CsvPersonProviderTest()
        { 
            _sut = new CsvPersonProvider(_loggerMock, _readerMock, _fileHandlerMock, _settingsMock);
        }

        private void SetupReaderMock()
        {
            _readerMock
            .ReadFromCsvAsync(Arg.Any<Func<IEnumerable<string>, int, CsvPerson>>(), Arg.Any<string>())
            .Returns(Task.FromResult(_persons));
        }

        private readonly List<CsvPerson> _persons =
        [
            new()
            {
                Id = 1,
                Name = "Max",
                LastName = "Mustermann",
                Color = Color.Blau
            },

            new()
            {
                Id = 2,
                Name = "Hans",
                LastName = "Hansen",
                Color = Color.Rot
            },

            new()
            {
                Id = 3,
                Name = "Lenny",
                LastName = "Lensen",
                Color = Color.Violett
            },

            new()
            {
                Id = 4,
                Name = "Carl",
                LastName = "Carlsen",
                Color = Color.Blau
            },

            new()
            {
                Id = 5,
                Name = "Maria",
                LastName = "Magdalena",
                Color = Color.Türkis
            }
        ];

        [Fact]
        public async Task GetAllPersons_Should_Return_All_Persons()
        {
            var expectedCount =_persons.Count;
            SetupReaderMock();
            

            var result = await _sut.GetAllPersonsAsync();

            result.Count.ShouldBe(expectedCount);
        }

        [Fact]
        public async Task GetAllPersons_Should_Log_Person_Count()
        {
            var expectedLogMessage = $"Found {_persons.Count} person";
            SetupReaderMock();

            await _sut.GetAllPersonsAsync();

            _loggerMock.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(expectedLogMessage)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Theory]
        [InlineData(Color.Blau, 2)]
        [InlineData(Color.Weiß, 0)]
        public async Task GetPersonsByColor_Should_Return_Persons_By_Color(Color queryColor, int expectedCount)
        {
            SetupReaderMock();
            var result = await _sut.GetPersonsByColorAsync(queryColor);
            result.Count.ShouldBe(expectedCount);
        }

        [Theory]
        [InlineData(Color.Blau, 2)]
        [InlineData(Color.Weiß, 0)]
        public async Task GetPersonsByColor_Should_Log_Count(Color queryColor, int expectedCount)
        {
            var expectedLogMessage = $"Found {expectedCount} persons for color '{queryColor.ToString()}'";
            SetupReaderMock();
            await _sut.GetPersonsByColorAsync(queryColor);

            _loggerMock.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(expectedLogMessage)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Fact]
        public async Task GetById_Should_Return_Correct_Person()
        {
            var expectedName = "Max";
            var id = 1;
            SetupReaderMock();

            var result = await _sut.GetPersonByIdAsync(id);
            result.HasValue.ShouldBeTrue();
            result.Value.Name.ShouldBe(expectedName);
        }

        [Fact]
        public async Task GetById_Should_Return_Maybe_None()
        {
            
            var id = 123;
            SetupReaderMock();

            var result = await _sut.GetPersonByIdAsync(id);
            result.HasValue.ShouldBeFalse();
        }

        [Fact]
        public async Task GetById_Should_Log_If_None_Found()
        {
            var id = 123;
            var expectedLogMessage = $"No entity of type '{nameof(CsvPerson)}' with key '{id}' found!";
            
            SetupReaderMock();

            await _sut.GetPersonByIdAsync(id);

            _loggerMock.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(expectedLogMessage)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
