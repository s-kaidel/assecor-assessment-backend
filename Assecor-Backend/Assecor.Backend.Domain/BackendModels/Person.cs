namespace Assecor.Backend.Domain.BackendModels
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string City { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
