using Assecor.Backend.Domain.ApiModels;
using Assecor.Backend.Domain.BackendModels;

namespace Assecor.Backend.Mappings
{
    public static class ApiPersonMapper
    {
        public static List<ApiPerson> MapFromDomainPersons(IEnumerable<Person> domainPersons)
        {
            var persons = domainPersons.Select(MapFromDomainPerson).ToList();
            return persons;
        }

        public static ApiPerson MapFromDomainPerson(Person domainPerson)
        {
            var person = new ApiPerson()
            {
                Id = domainPerson.Id,
                Name = domainPerson.Name,
                LastName = domainPerson.LastName,
                City = domainPerson.City,
                Color = domainPerson.Color.ToString()?.ToLower(),
                ZipCode = domainPerson.ZipCode
            };
            return person;
        }
    }
}
