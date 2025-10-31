using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;
using Assecor.Backend.Mappings.Interfaces;
using Assecor.Backend.Services;
using Assecor.Backend.Services.Contracts;
using NSubstitute.ReceivedExtensions;

namespace Assecor.Backend.Test.ServiceTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _sut;
        private readonly ICsvPersonProvider _providerMock = Substitute.For<ICsvPersonProvider>();
        private readonly IPersonMapper _personMapperMock = Substitute.For<IPersonMapper>();
        private readonly ICsvPersonDtoMapper _dtoMapperMock = Substitute.For<ICsvPersonDtoMapper>();

        private readonly CsvPerson _testPerson = new CsvPerson()
        {
            Name = "Walther",
            LastName = "Mein Gott"
        };

        public PersonServiceTest()
        {
            _sut = new PersonService(_providerMock, _personMapperMock, _dtoMapperMock);
        }

        private void SetupMocks()
        {
            _personMapperMock.MapFromCsvPerson(Arg.Any<CsvPerson>()).Returns(new Person());
            _personMapperMock.MapFromCsvPersons(Arg.Any<List<CsvPerson>>()).Returns([]);
            _providerMock.GetPersonsByColorAsync(Arg.Any<Color>()).Returns([_testPerson]);
            _providerMock.GetAllPersonsAsync().Returns([_testPerson]);
            _providerMock.GetPersonByIdAsync(Arg.Any<int>()).Returns(Maybe.From(_testPerson));
        }

        [Fact]
        public async Task GetAllPersons_Should_Make_Correct_Calls()
        {
            SetupMocks();
            var expectedProviderCalls = 1;
            var expectedMapperCalls = 1;

            await _sut.GetAllPersonsAsync();

            await _providerMock.Received(expectedProviderCalls).GetAllPersonsAsync();
            _personMapperMock.ReceivedWithAnyArgs(expectedMapperCalls).MapFromCsvPersons(Arg.Is<List<CsvPerson>>([_testPerson]));
        }

        [Fact]
        public async Task GetPersonsByColor_Should_Make_Correct_Calls()
        {
            SetupMocks();
            var color = Color.Blau;
            var expectedProviderCalls = 1;
            var expectedMapperCalls = 1;

            await _sut.GetPersonsByColorAsync(color);

            await _providerMock.Received(expectedProviderCalls).GetPersonsByColorAsync(Arg.Is(color));
            _personMapperMock.ReceivedWithAnyArgs(expectedMapperCalls).MapFromCsvPersons(Arg.Is<List<CsvPerson>>([_testPerson]));
        }

        [Fact]
        public async Task GetPersonById_Should_Make_Correct_Calls()
        {
            SetupMocks();
            var id = 1;
            var expectedProviderCalls = 1;
            var expectedMapperCalls = 1;

            await _sut.GetPersonByIdAsync(id);

            await _providerMock.Received(expectedProviderCalls).GetPersonByIdAsync(Arg.Is(id));
            _personMapperMock.Received(expectedMapperCalls).MapFromCsvPerson(Arg.Is(_testPerson));
        }
    }
}
