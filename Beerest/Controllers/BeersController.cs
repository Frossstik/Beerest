using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beerest;
using Beerest.Models;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Produces("application/json")]
    public class BeersController : Controller
    {
        private readonly AppDbContext _context;

        public BeersController(AppDbContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Beers>> PostBeer([FromBody] Beers beer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
