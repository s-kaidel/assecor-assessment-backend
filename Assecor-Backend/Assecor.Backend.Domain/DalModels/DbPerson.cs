namespace Assecor.Backend.Domain.DalModels
{
    public class DbPerson
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string City { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
