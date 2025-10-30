using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.Test
{
    public class CsvFileLocationHandlerTest
    {
        private string _validFileName = "persons.csv";
        private ICsvFileLocationHandler? _sut;
        private readonly string _filesDirectory = Path.Combine(AppContext.BaseDirectory, "CsvReaderTest", "TestFiles");
        private readonly IOptions<CsvSettings> _settings = Substitute.For<IOptions<CsvSettings>>();

        [Fact]
        public void Should_Return_Persons_File_Path()
        {
            _settings.Value.Returns(new CsvSettings()
            {
                DirectoryPath = _filesDirectory,
                Files = new()
                {
                    Persons = _validFileName
                }
            });
            var expectedFilePath = Path.Combine(AppContext.BaseDirectory, "CsvReaderTest", "TestFiles", _validFileName);

            _sut = new CsvFileLocationHandler(_settings);

            var filePath = _sut.GetPersonsFilePath();
            filePath.ShouldBe(expectedFilePath);
        }

        [Fact]
        public void Should_Throw_On_Wrong_File_Name()
        {
            var fileName = "random.csv";
            _settings.Value.Returns(new CsvSettings()
            {
                DirectoryPath = _filesDirectory,
                Files = new()
                {
                    Persons = fileName
                }
            });
            _sut = new CsvFileLocationHandler(_settings);
            var expectedError =
                $"Csv file '{fileName}' not found, please review provided file name in appSettings";

            var act = () => _sut.GetPersonsFilePath();

            act.ShouldThrow<FileNotFoundException>(expectedError);
        }

        [Fact]
        public void Should_Throw_On_Wrong_Directory_Name()
        {
            _settings.Value.Returns(new CsvSettings()
            {
                DirectoryPath = "someDirectory",
                Files = new()
                {
                    Persons = _validFileName
                }
            });
            _sut = new CsvFileLocationHandler(_settings);
            var expectedError = "Could not find file directory, please review provided directory in appSettings";

            var act = () => _sut.GetPersonsFilePath();

            act.ShouldThrow<DirectoryNotFoundException>(expectedError);
        }
    }
}
