using Assecor.Backend.CsvAccess;
using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Exceptions;

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

        [Fact]
        public async Task Should_Skip_First_Row()
        {
            var expectedLine1 = "Hans, Habicht, 91074 Falkendorf, 2";
            var expectedLine2 = "Gundula, Geier, 91074 Niederndorf, 3";

            var filePath = GetTempCsvFilePath(expectedLine1);
            var expectedLength = 2;
           
            var csvPerson = new CsvPersonDto()
            {
                Name = "Gundula",
                LastName = "Geier",
                Location = "91074 Niederndorf",
                Color = 3
            };

            await _sut.AppendToCsvAsync(filePath, [csvPerson]);
            
            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Length.ShouldBe(expectedLength);
            lines[0].ShouldBe(expectedLine1);
            lines[1].ShouldBe(expectedLine2);

            File.Delete(filePath);
        }

        [Fact]
        public async Task Should_Write_Header_Row()
        {
            var expectedLine1 = "Name, LastName, Location, Color";
            var expectedLine2 = "Gundula, Geier, 91074 Niederndorf, 3";

            var filePath = GetTempCsvFilePath();
            var expectedLength = 2;
           
            var csvPerson = new CsvPersonDto()
            {
                Name = "Gundula",
                LastName = "Geier",
                Location = "91074 Niederndorf",
                Color = 3
            };

            await _sut.AppendToCsvAsync(filePath, [csvPerson], false, true);
            
            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Length.ShouldBe(expectedLength);
            lines[0].ShouldBe(expectedLine1);
            lines[1].ShouldBe(expectedLine2);

            File.Delete(filePath);
        }

        [Fact]
        public async Task Should_Not_Write_Over_Existing_Header_Row()
        {
            var expectedLine1 = "Name, LastName, Location, Color";
            var expectedLine2 = "Gundula, Geier, 91074 Niederndorf, 3";

            var filePath = GetTempCsvFilePath(expectedLine1);
            var expectedLength = 2;
           
            var csvPerson = new CsvPersonDto()
            {
                Name = "Gundula",
                LastName = "Geier",
                Location = "91074 Niederndorf",
                Color = 3
            };

            await _sut.AppendToCsvAsync(filePath, [csvPerson], true, false);
            
            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Length.ShouldBe(expectedLength);
            lines[0].ShouldBe(expectedLine1);
            lines[1].ShouldBe(expectedLine2);

            File.Delete(filePath);
        }

        [Fact]
        public async Task Should_Throw_Exception()
        {
            var errorFilePath = "abc" + GetTempCsvFilePath();
            var error = "An error occurred while trying to write data to the csv file:";

            var act = async () => await _sut.AppendToCsvAsync(errorFilePath, []);

            var ex = await act.ShouldThrowAsync<CsvWriterException>();
            ex.Message.ShouldStartWith(error);
            ex.InnerException?.ShouldBeOfType<FileNotFoundException>();
        }
    }
}
