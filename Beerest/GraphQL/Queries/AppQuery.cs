using AutoMapper;
using Beerest.GraphQL.Types;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;

namespace Beerest.GraphQL.Queries
{
    public class AppQuery
    {
        private readonly IBeersRepository _beersRepository;
        private readonly IBarsRepository _barsRepository;
        private readonly IPersonsRepository _personsRepository;
        private readonly IMapper _mapper;

        public AppQuery(IBeersRepository beersRepository, IBarsRepository barsRepository, IPersonsRepository personsRepository, IMapper mapper)
        {
            _beersRepository = beersRepository;
            _barsRepository = barsRepository;
            _personsRepository = personsRepository;
            _mapper = mapper;
        }

        public async Task<List<BeersType>> GetBeers()
        {
            var beers = await _beersRepository.GetAllAsync();
            return _mapper.Map<List<BeersType>>(beers);
        }

        public async Task<List<BarsType>> GetBars()
        {
            var bars = await _barsRepository.GetAllAsync();
            return _mapper.Map<List<BarsType>>(bars);
        }

        public async Task<List<PersonsType>> GetPersons()
        {
            var persons = await _personsRepository.GetAllAsync();
            return _mapper.Map<List<PersonsType>>(persons);
        }
    }
}
