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
    public class BarsController : Controller
    {
        private readonly AppDbContext _context;

        public BarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bars>>> GetBars()
        {
            return await _context.bars.ToListAsync();
        }

        // GET: api/Bars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bars>> GetBar(int id)
        {
            var bar = await _context.bars.FindAsync(id);

            if (bar == null)
            {
                return NotFound();
            }

            return bar;
        }

        // POST: api/Bars
        [HttpPost]
        public async Task<ActionResult<Bars>> PostBar([FromBody] Bars bar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
