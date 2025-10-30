using Assecor.Backend.Domain.ApiModels;
using Assecor.Backend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Assecor.Backend.Api.Responses;
using Assecor.Backend.Mappings;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController(IPersonService personService, IValidationService validationService) : RestServerControllerBase
    {
        private readonly IPersonService _personService = personService;
        private readonly IValidationService _validationService = validationService;

        /// <summary>
        /// Returns all available persons from the data storage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<ApiPerson>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RestServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonsAsync()
        {
            var persons = await _personService.GetAllPersonsAsync();
            var apiPersons = ApiPersonMapper.MapFromDomainPersons(persons);
            return RestServerOk(apiPersons);
        }

        /// <summary>
        /// Returns all persons matching given color
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        [HttpGet]
        [Route("color/{color}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<ApiPerson>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RestServerErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RestServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonsByColorAsync(int color)
        {
            if (!_validationService.IsValidEnumValue<Color>(color))
            {
                return RestServerBadRequest($"Color {color} is not valid");
            }
            var persons = await _personService.GetPersonsByColorAsync((Color)color);
            var apiPersons = ApiPersonMapper.MapFromDomainPersons(persons);
            return RestServerOk(apiPersons);

        }

        /// <summary>
        /// Returns the person matching given id
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RestServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonByIdAsync(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            var apiPerson = person.Map(ApiPersonMapper.MapFromDomainPerson);
            return MapToResult(apiPerson, $"No person with id '{id}' could be found!");
        }
    }
}
