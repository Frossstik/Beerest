using AutoMapper;
using Beerest.Mapping.DTO;
using Beerest.Models;

namespace Beerest.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Beers, BeersDto>().ReverseMap();

            CreateMap<BarsDto, Bars>()
            .ForMember(dest => dest.Beer, opt => opt.Ignore());

            CreateMap<PersonsDto, Persons>()
            .ForMember(dest => dest.Bar, opt => opt.Ignore());
        }
    }
}
