using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Services;
using Hirealdoor.Setting;
using Hirealdoor.Validators;
using Microsoft.AspNetCore.Mvc;


namespace Hirealdoor.Controllers
{
    [Authorize(Type = [1])]
    [ApiController]
    [Route(ApiRoutes.Website.Person)]
    public class PersonController(IPersonService personService, IRedisService redisService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PersonIndexDto filter)
        {
            var result = await redisService.GetValueAsync<PaginateResponseDto<PersonResponseDto>>("person_result");
            if (result != null)
            {
                return Ok(result);
            }

            result = await personService.Filter(filter);
            await redisService.SetValueAsync("person_result", result, TimeSpan.FromMinutes(10));

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