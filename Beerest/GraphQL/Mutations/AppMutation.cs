using AutoMapper;
using Beerest.GraphQL.Types;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using Beerest.Models;

namespace Beerest.GraphQL.Mutations
{
    public class AppMutation
    {
        private readonly IBarsRepository _barsRepository;
        private readonly IBeersRepository _beersRepository;
        private readonly IPersonsRepository _personsRepository;
        private readonly IMapper _mapper;

        public AppMutation(IBarsRepository barsRepository, IBeersRepository beersRepository, IPersonsRepository personsRepository, IMapper mapper)
        {
            _barsRepository = barsRepository;
            _beersRepository = beersRepository;
            _personsRepository = personsRepository;
            _mapper = mapper;
        }

        public async Task<BarsType> CreateBar(BarsDto input)
        {
            var bar = _mapper.Map<Bars>(input);
            await _barsRepository.CreateAsync(bar);
            return _mapper.Map<BarsType>(bar);
        }

        public async Task<BarsType> UpdateBar(int id, BarsDto input)
        {
            var bar = await _barsRepository.GetByIdAsync(id);
            if (bar == null) throw new Exception("Bar not found");
            _mapper.Map(input, bar);
            await _barsRepository.UpdateAsync(bar);
            return _mapper.Map<BarsType>(bar);
        }

        // Delete Bar
        public async Task<bool> DeleteBar(int id)
        {
            await _barsRepository.DeleteAsync(id);
            return true;
        }

        public async Task<BeersType> CreateBeer(BeersDto input)
        {
            var beer = _mapper.Map<Beers>(input);
            await _beersRepository.CreateAsync(beer);
            return _mapper.Map<BeersType>(beer);
        }

        public async Task<BeersType> UpdateBeer(int id, BeersDto input)
        {
            var beer = await _beersRepository.GetByIdAsync(id);
            if (beer == null) throw new Exception("Beer not found");
            _mapper.Map(input, beer);
            await _beersRepository.UpdateAsync(beer);
            return _mapper.Map<BeersType>(beer);
        }

        public async Task<bool> DeleteBeer(int id)
        {
            await _beersRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PersonsType> CreatePerson(PersonsDto input)
        {
            var person = _mapper.Map<Persons>(input);
            await _personsRepository.CreateAsync(person);
            return _mapper.Map<PersonsType>(person);
        }

        public async Task<PersonsType> UpdatePerson(int id, PersonsDto input)
        {
            var person = await _personsRepository.GetByIdAsync(id);
            if (person == null) throw new Exception("Person not found");
            _mapper.Map(input, person);
            await _personsRepository.UpdateAsync(person);
            return _mapper.Map<PersonsType>(person);
        }

        public async Task<bool> DeletePerson(int id)
        {
            await _personsRepository.DeleteAsync(id);
            return true;
        }
    }
}
