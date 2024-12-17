using AutoMapper;
using Beerest.GraphQL.Types;
using Beerest.Mapping.DTO;
using Beerest.Models;
using CheckService;

namespace Beerest.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Beers, BeersDto>().ReverseMap();

            CreateMap<BarsDto, Bars>()
            .ForMember(dest => dest.Beers, opt => opt.Ignore());

            CreateMap<PersonsDto, Persons>()
            .ForMember(dest => dest.Bar, opt => opt.Ignore());

            CreateMap<Beers, BeersType>().ReverseMap();
            CreateMap<Persons, PersonsType>()
                .ForMember(dest => dest.Bar, opt => opt.MapFrom(src => src.Bar));
            CreateMap<Bars, BarsType>()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.Beers));

            CreateMap<CreateCheckMessage, CreateCheckRequest>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CheckItemDto, CheckItem>();
        }
    }
}
