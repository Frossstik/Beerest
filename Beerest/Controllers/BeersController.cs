using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Beerest.Models;
using X.PagedList.EF;
using Beerest.HAL;

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

        // GET: api/Beers?page=1&pageSize=5
        [HttpGet]
        [Produces("application/hal+json")]
        public async Task<ActionResult> GetBeers(int page = 1, int pageSize = 5)
        {
            var beersQuery = _context.beers.AsQueryable();
            var pagedBeers = await beersQuery.ToPagedListAsync(page, pageSize);

            var response = new BeerHalResponse
            {
                Beers = pagedBeers.ToList()
            };

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            response.Links.Add("self", new HalLink($"{baseUrl}?page={page}&pageSize={pageSize}"));

            if (pagedBeers.HasNextPage)
            {
                response.Links.Add("next", new HalLink($"{baseUrl}?page={page + 1}&pageSize={pageSize}"));
            }

            if (pagedBeers.HasPreviousPage)
            {
                response.Links.Add("prev", new HalLink($"{baseUrl}?page={page - 1}&pageSize={pageSize}"));
            }

            return Ok(response);
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