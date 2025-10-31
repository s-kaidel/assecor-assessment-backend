using Assecor.Backend.CsvAccess;
using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;

namespace Assecor.Backend.Test.CsvTest
{
    public class CsvWriterTest
    {
        private readonly ICsvWriter<CsvPerson> _sut = new CsvWriter<CsvPerson>();

        [Fact]
        public async Task Should_Append_To_Csv()
        {
            var csvPerson = new CsvPerson()
            {
                Id = 1,
                Name = "Hans",
                LastName = "Habicht",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Rot
            };
        }
    }
}
