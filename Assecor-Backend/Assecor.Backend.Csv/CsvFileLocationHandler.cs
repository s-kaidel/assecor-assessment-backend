using Assecor.Backend.Configuration;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.CsvAccess
{
    public class CsvFileLocationHandler(IOptions<CsvSettings> settings) : ICsvFileLocationHandler
    {
        private readonly CsvSettings _settings = settings.Value;
        private readonly string _baseDirectory = settings.Value.DirectoryPath ?? AppContext.BaseDirectory;

        private string GetCsvFilePath(string fileName)
        {
            var filePath = Path.Combine(_baseDirectory, fileName);
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
