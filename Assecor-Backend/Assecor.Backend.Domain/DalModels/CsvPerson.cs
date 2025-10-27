using Assecor.Backend.Domain.Enums;

namespace Assecor.Backend.Domain.DalModels
{
    public class CsvPerson
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string City { get; set; } = string.Empty;
        public Color Color { get; set; }
    }
}
