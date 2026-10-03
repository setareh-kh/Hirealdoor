using AutoMapper;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor;

public class MapprProfile : Profile
{
    public MapprProfile()
    {
        //mapping requestDtos to models
        CreateMap<CreatePersonDto, Person>().ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Person, PersonResponseDto>();
    }
}