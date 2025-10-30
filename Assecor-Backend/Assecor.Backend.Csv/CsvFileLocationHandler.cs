using Assecor.Backend.Configuration;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.CsvAccess
{
    public class CsvFileLocationHandler(IOptions<CsvSettings> settings) : ICsvFileLocationHandler
    {
        private readonly CsvSettings _settings = settings.Value;

        private string GetCsvFilePath(string fileName)
        {
            var baseDirectory = !string.IsNullOrEmpty(_settings.DirectoryPath) 
                ? _settings.DirectoryPath 
                : AppContext.BaseDirectory;

            var filePath = Path.Combine(baseDirectory, fileName);
            var validDirectory = Directory.Exists(baseDirectory);
            var validFileName = File.Exists(filePath);


            return validDirectory
                    ? validFileName
                        ? filePath
                        : throw new FileNotFoundException($"Csv file '{fileName}' not found, please review provided file name in appSettings")
                    : throw new DirectoryNotFoundException("Could not find file directory, please review provided directory in appSettings");
        }

        public string GetPersonsFilePath()
        {
            return GetCsvFilePath(_settings.Files.Persons);
        }
    }
}
