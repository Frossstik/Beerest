using AutoMapper;
using Beerest.GraphQL.Types;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using Beerest.Models;
using CheckService;

namespace Beerest.GraphQL.Mutations
{
    public class AppMutation
    {
        private readonly IBarsRepository _barsRepository;
        private readonly IBeersRepository _beersRepository;
        private readonly IPersonsRepository _personsRepository;
        private readonly IMapper _mapper;

        public AppMutation(
            IBarsRepository barsRepository, 
            IBeersRepository beersRepository, 
            IPersonsRepository personsRepository, 
            IMapper mapper)
        {
            _barsRepository = barsRepository;
            _beersRepository = beersRepository;
            _personsRepository = personsRepository;
            _mapper = mapper;
        }

        public async Task<BarsType> CreateBar(BarsDto input)
        {
            var bar = _mapper.Map<Bars>(input);
            var beers = new List<Beers>();
            foreach (var beerId in input.BeerIds)
            {
                var beer = await _beersRepository.GetByIdAsync(beerId);
                if (beer == null)
                {
                    throw new Exception($"Пива под номером {beerId} нету!");
                }
                beers.Add(beer);
            }
            bar.Beers = beers;
            await _barsRepository.CreateAsync(bar);
            return _mapper.Map<BarsType>(bar);
        }

        public async Task<BarsType> UpdateBar(int id, BarsDto input, [Service] AppDbContext context)
        {
            var bar = await context.bars.FindAsync(id);
            if (bar == null) throw new Exception("Бар не найден!");
            _mapper.Map(input, bar);
            await context.SaveChangesAsync();
            return _mapper.Map<BarsType>(bar);
        }

        public async Task<bool> DeleteBar(int id, [Service] AppDbContext context)
        {
            var bar = await context.bars.FindAsync(id);
            if (bar == null) return false;
            context.bars.Remove(bar);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<BeersType> CreateBeer(BeersDto input, [Service] AppDbContext context)
        {
            var beer = _mapper.Map<Beers>(input);
            await context.beers.AddAsync(beer);
            await context.SaveChangesAsync();
            return _mapper.Map<BeersType>(beer);
        }

        public async Task<BeersType> UpdateBeer(int id, BeersDto input, [Service] AppDbContext context)
        {
            var beer = await context.beers.FindAsync(id);
            if (beer == null) throw new Exception("Пивас не найден!");
            _mapper.Map(input, beer);
            await context.SaveChangesAsync();
            return _mapper.Map<BeersType>(beer);
        }

        public async Task<bool> DeleteBeer(int id, [Service] AppDbContext context)
        {
            var beer = await context.beers.FindAsync(id);
            if (beer == null) return false;
            context.beers.Remove(beer);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PersonsType> CreatePerson(PersonsDto input)
        {
            var person = _mapper.Map<Persons>(input);

            if (input.BarId.HasValue)
            {
                var bar = await _barsRepository.GetByIdAsync(input.BarId.Value);
                if (bar == null)
                {
                    throw new Exception("Бар не найден!");
                }
                person.Bar = bar;
            }
            await _personsRepository.CreateAsync(person);
            return _mapper.Map<PersonsType>(person);
        }

        public async Task<PersonsType> UpdatePerson(int id, PersonsDto input, [Service] AppDbContext context)
        {
            var person = await context.persons.FindAsync(id);
            if (person == null) throw new Exception("Челик не найден!");
            _mapper.Map(input, person);
            await context.SaveChangesAsync();
            return _mapper.Map<PersonsType>(person);
        }

        public async Task<bool> DeletePerson(int id, [Service] AppDbContext context)
        {
            var person = await context.persons.FindAsync(id);
            if (person == null) return false;
            context.persons.Remove(person);
            await context.SaveChangesAsync();
            return true;
        }

        private class FileGeneratedMessage
        {
            public string FilePath { get; set; }
        }
    }
}
