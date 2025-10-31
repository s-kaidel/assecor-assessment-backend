using Assecor.Backend.CsvAccess;
using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Domain.Dto;

namespace Assecor.Backend.Test.CsvTest
{
    public class CsvWriterTest
    {
        private readonly ICsvWriter<CsvPersonDto> _sut = new CsvWriter<CsvPersonDto>();
        private string GetTempCsvFilePath(string content = "")
        {
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
            File.WriteAllText(filePath, content);
            return filePath;
        }
        
        [Fact]
        public async Task Should_Append_To_Csv()
        {
            var filePath = GetTempCsvFilePath();
            var expectedLength = 1;
            var expectedLine = "Hans, Habicht, 91074 Falkendorf, 2";

            var csvPerson = new CsvPersonDto()
            {
                Name = "Hans",
                LastName = "Habicht",
                Location = "91074 Falkendorf",
                Color = 2
            };

            await _sut.AppendToCsvAsync(filePath, [csvPerson]);
            
            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Length.ShouldBe(expectedLength);
            lines[0].ShouldBe(expectedLine);

            File.Delete(filePath);
        }

        [Fact]
        public async Task Should_Append_Multiple_Records_To_Csv()
        {
            var filePath = GetTempCsvFilePath();
            var expectedLength = 2;
            var expectedLine1 = "Hans, Habicht, 91074 Falkendorf, 2";
            var expectedLine2 = "Gundula, Geier, 91074 Niederndorf, 3";

            var csvPerson1 = new CsvPersonDto()
            {
                Name = "Hans",
                LastName = "Habicht",
                Location = "91074 Falkendorf",
                Color = 2
            };
            var csvPerson2 = new CsvPersonDto()
            {
                Name = "Gundula",
                LastName = "Geier",
                Location = "91074 Niederndorf",
                Color = 3
            };

            await _sut.AppendToCsvAsync(filePath, [csvPerson1, csvPerson2]);

            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Length.ShouldBe(expectedLength);
            lines[0].ShouldBe(expectedLine1);
            lines[1].ShouldBe(expectedLine2);

            File.Delete(filePath);
        }
    }
}
