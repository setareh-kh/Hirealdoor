using AutoMapper;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;
using Hirealdoor.Repositories;


namespace Hirealdoor.Services.Service
{
    public class PersonService(IPersonRepository personRepository, IUserRepository userRepository, IMapper mapper)
        : IPersonService
    {
        public async Task<PaginateResponseDto<PersonResponseDto>> Filter(PersonIndexDto filter)
        {
            var res = await personRepository.Filter(filter);
            return new PaginateResponseDto<PersonResponseDto>()
            {
                Page = res.Page,
                Pager = res.Pager,
                Total = res.Total,
                Pages = res.Pages,
                Items = res.Items?.Select(mapper.Map<PersonResponseDto>).ToList() ?? []
            };
        }

        //use case:complete register
        public async Task<bool> CreatePersonAsync(CreatePersonDto createPersonDto)
        {
            var person = mapper.Map<Person>(createPersonDto);
            var existingUser = await userRepository.GetByIdAsync(createPersonDto.UserId);
            if (existingUser == null)
                throw new Exception("please register first");
            var existingPerson = await personRepository.GetByIdAsync(createPersonDto.UserId);
            if (existingPerson != null)
                throw new Exception("Person is already exists");
            await personRepository.InsertAsync(person);
            
            await personRepository.SyncLanguages(person, createPersonDto.LangIds ?? "");
            
            return true;
        }

        public async Task<bool> CompleteDetail()
        {
            return true;
        }
    }
}