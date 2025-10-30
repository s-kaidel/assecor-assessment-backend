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
            return File.Exists(filePath) 
                ? filePath 
                : throw new FileNotFoundException($"Csv file '{fileName}' not found, please review provided file name and file directory in appSettings");
        }

        public string GetPersonsFilePath()
        {
            return GetCsvFilePath(_settings.Files.Persons);
        }
    }
}
