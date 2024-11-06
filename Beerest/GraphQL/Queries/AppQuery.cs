using AutoMapper;
using Beerest.GraphQL.Types;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<BeersType>> GetBeers([Service] AppDbContext context)
        {
            var beers = await context.beers.ToListAsync();
            return _mapper.Map<List<BeersType>>(beers);
        }

        public async Task<List<BeersType>> GetBeer(int id, [Service] AppDbContext context)
        {
            var beer = await context.beers.FirstOrDefaultAsync(b => b.Id == id);
            return _mapper.Map<List<BeersType>>(beer);
        }

        public async Task<List<BarsType>> GetBars([Service] AppDbContext context)
        {
            var bars = await context.bars.Include(b => b.Beers).ToListAsync();
            return _mapper.Map<List<BarsType>>(bars);
        }

        public async Task<List<BarsType>> GetBar(int id, [Service] AppDbContext context)
        {
            var bar = await context.bars.Include(b => b.Beers).FirstOrDefaultAsync(b => b.Id == id);
            return _mapper.Map<List<BarsType>>(bar);
        }

        public async Task<List<PersonsType>> GetPersons([Service] AppDbContext context)
        {
            var persons = await context.persons.Include(p => p.Bar).ThenInclude(b => b.Beers).ToListAsync();
            return _mapper.Map<List<PersonsType>>(persons);
        }

        public async Task<PersonsType> GetPerson(int id, [Service] AppDbContext context)
        {
            var person = await context.persons.Include(p => p.Bar).ThenInclude(b => b.Beers).FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<PersonsType>(person);
        }
    }
}
