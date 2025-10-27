using System.Net.Mime;
using Assecor.Backend.Domain.ApiModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Mapping;
using Assecor.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Assecor.Backend.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController(IPersonService personService, ILogger<PersonController> logger) : ControllerBase
    {
        /// <summary>
        /// Returns all persons from the data storage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<ApiPerson>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonsAsync()
        {
            var persons = await personService.GetAllPersonsAsync();
            var apiPersons = ApiPersonMapper.MapFromDomainPersons(persons);
            return new OkObjectResult(apiPersons);
        }

        [HttpGet]
        [Route("color/{color}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<ApiPerson>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonsByColorAsync(int color)
        {
            if (Enum.IsDefined(typeof(Color), color))
            {
                var persons = await personService.GetPersonsByColorAsync((Color)color);
                var apiPersons = ApiPersonMapper.MapFromDomainPersons(persons);
                return new OkObjectResult(apiPersons);
            }

            return new BadRequestObjectResult($"Color {color} is not valid");
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiPerson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonByIdAsync(int id)
        {
            ApiPerson? apiPerson = null;
            var person = await personService.GetPersonByIdAsync(id);
            if (person != null)
            {
                apiPerson = ApiPersonMapper.MapFromDomainPerson(person);
            }

            return new OkObjectResult(apiPerson);
        }
        
    }
}
