using Hirealdoor.Dtos.Requests;
using Hirealdoor.Services;
using Microsoft.AspNetCore.Mvc;


namespace Hirealdoor.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController(IPersonService personService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreatePerson([FromForm] CreatePersonDto dto)
        {
            var result = await personService.CreatePersonAsync(dto);
            //if (result)
            return Ok(result);
        }
    }
}