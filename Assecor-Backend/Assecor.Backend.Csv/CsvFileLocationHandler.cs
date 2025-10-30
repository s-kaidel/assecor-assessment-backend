using Assecor.Backend.Configuration;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.CsvAccess
{
    public class CsvFileLocationHandler(IOptions<CsvSettings> options) : ICsvFileLocationHandler
    {
        private readonly string _baseDirectory = options.Value.DirectoryPath ?? AppContext.BaseDirectory;

        public string GetCsvFilePath(string fileName)
        {
            var filePath = Path.Combine(_baseDirectory, fileName);
            return File.Exists(filePath) 
                ? filePath 
                : throw new FileNotFoundException($"Csv file '{fileName}' not found, please review provided file name and file directory in appSettings");
        }
    }
}
