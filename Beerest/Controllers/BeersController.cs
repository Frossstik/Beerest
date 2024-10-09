using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Beerest.Models;
using AutoMapper;
using Beerest.Mapping.DTO;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Produces("application/json")]
    public class BeersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Beers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beers>>> GetBeers()
        {
            return await _context.beers.ToListAsync();
        }

        // GET: api/Beers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beers>> GetBeer(int id)
        {
            var beer = await _context.beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            return beer;
        }

        // POST: api/Beers
        [HttpPost]
        public async Task<ActionResult<Beers>> PostBeer(BeersDto beersDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var beer = _mapper.Map<Beers>(beersDto);

            _context.beers.Add(beer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeer), new { id = beer.Id }, beer);
        }

        // PUT: api/Beers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeer(int id, [FromBody] Beers beer)
        {
            if (id != beer.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(beer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Beers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            var beer = await _context.beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            _context.beers.Remove(beer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeersExists(int id)
        {
            return _context.beers.Any(e => e.Id == id);
        }
    }

}