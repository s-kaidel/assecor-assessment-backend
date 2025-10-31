namespace Assecor.Backend.Domain.Dto
{
    public class CsvPersonDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public int? Color { get; set; }
    }
}
