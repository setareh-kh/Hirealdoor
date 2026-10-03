using Hirealdoor.Dtos.Requests;
using Hirealdoor.Services;
using Microsoft.AspNetCore.Mvc;


namespace Hirealdoor.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController(IPersonService personService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PersonIndexDto filter)
        {
            var result = await personService.Filter(filter);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePerson([FromForm] CreatePersonDto dto)
        {
            var result = await personService.CreatePersonAsync(dto);
            //if (result)
            return Ok(result);
        }
    }
}