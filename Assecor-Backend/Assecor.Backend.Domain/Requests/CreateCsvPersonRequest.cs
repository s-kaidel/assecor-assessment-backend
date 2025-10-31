namespace Assecor.Backend.Domain.Requests
{
    public class CreateCsvPersonRequest
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? ZipCode { get; set; }
        public string? City { get; set; }
        public int? Color { get; set; }
    }
}
