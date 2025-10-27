using Assecor.Backend.Domain.Mapping;
using Assecor.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Assecor.Backend.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController(IPersonService personService) : ControllerBase
    {
        public IActionResult GetPersons()
        {
            var persons = personService.GetPersons();
            var apiPersons = ApiPersonMapper.MapFromDomainPersons(persons);
            return new OkObjectResult(apiPersons);
        }
    }
}
