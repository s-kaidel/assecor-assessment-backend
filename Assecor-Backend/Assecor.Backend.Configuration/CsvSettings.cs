namespace Assecor.Backend.Configuration
{
    public class CsvSettings
    {
        public string? DirectoryPath { get; set; } = string.Empty;
        public CsvFiles Files { get; set; } = new();
    }
}
