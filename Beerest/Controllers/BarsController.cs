using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beerest;
using Beerest.Models;
using Beerest.Mapping.DTO;
using AutoMapper;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Produces("application/json")]
    public class BarsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BarsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Bars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bars>>> GetBars()
        {
            var bars = await _context.bars
                .Include(b => b.Beer)
                .ToListAsync();

            return Ok(bars);
        }

        // GET: api/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bars>> GetBar(int id)
        {
            var bar = await _context.bars.Include(b => b.Beer).FirstOrDefaultAsync(b => b.Id == id);


            if (bar == null)
            {
                return NotFound();
            }

            return bar;
        }

        // POST: api/Bars
        [HttpPost]
        public async Task<ActionResult<Bars>> PostBar(BarsDto barsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bar = _mapper.Map<Bars>(barsDto);

            var beer = await _context.beers.FindAsync(barsDto.BeerId);

            if (beer == null)
            {
                return NotFound("Beer is not found!");
            }

            bar.Beer = beer;

            _context.bars.Add(bar);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBar), new { id = bar.Id }, bar);
        }

        // PUT: api/Bars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBar(int id, [FromBody] Bars bar)
        {
            if (id != bar.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(bar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarExists(id))
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

        // DELETE: api/Bars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBar(int id)
        {
            var bar = await _context.bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            _context.bars.Remove(bar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarExists(int id)
        {
            return _context.bars.Any(e => e.Id == id);
        }
    }
}
