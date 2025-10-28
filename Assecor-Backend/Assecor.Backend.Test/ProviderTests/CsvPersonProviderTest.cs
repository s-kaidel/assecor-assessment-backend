using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Dal.Provider;
using Assecor.Backend.Domain.DalModels;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Test.ProviderTests
{
    public class CsvPersonProviderTest
    {
        private readonly ICsvPersonProvider _sut;
        private readonly ICsvReader<CsvPerson> _readerMock = Substitute.For<ICsvReader<CsvPerson>>();

        public CsvPersonProviderTest()
        {
            var logger = Substitute.For<ILogger<CsvPersonProvider>>();
            _sut = new CsvPersonProvider(logger, _readerMock);
        }

        [Fact]
        public async Task Should_Return_All_Persons()
        {
            var expectedCount = 1;
            var persons = new List<CsvPerson>
            {
                new()
                {
                    Id = 1,
                    Name = "Max",
                    LastName = "Mustermann"
                }
            };

            _readerMock
                .ReadFromCsvAsync(Arg.Any<Func<IEnumerable<string>, int, CsvPerson>>())
                .Returns(Task.FromResult(persons));

            var result = await _sut.GetAllPersonsAsync();

            result.Count.ShouldBe(expectedCount);
        }
    }
}
