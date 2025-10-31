namespace Assecor.Backend.Domain.ApiModels
{
    public class ApiPerson
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Color { get; set; }
    }
}
