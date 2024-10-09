using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beerest;
using Beerest.Models;
using System.Dynamic;
using AutoMapper;
using Beerest.Mapping.DTO;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Produces("application/json")]
    public class PersonsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PersonsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private dynamic Paginate(string url, int index, int count, int total)
        {
            dynamic links = new ExpandoObject();
            links.self = new { href = url };
            links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
            links.first = new { href = $"{url}?index=0&count={count}" };
            if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
            if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
            return links;
        }

        // GET: api/Persons
        [HttpGet]
        [Produces("application/hal+json")]
        public async Task<ActionResult<IEnumerable<Persons>>> GetPersons(int index = 0, int count = 5)
        {
            var total = await _context.persons.CountAsync();

            var items = await _context.persons
                .Skip(index)
                .Take(count)
                .Include(p => p.Bar)
                .ThenInclude(b => b.Beer)
                .ToListAsync();

          
            var _links = Paginate("/api/persons", index, count, total);

            var result = new
            {
                _links,
                index,
                count,
                total,
                items
            };

            return Ok(result);
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persons>> GetPerson(int id)
        {
            var person = await _context.persons.Include(p => p.Bar).ThenInclude(b => b.Beer).FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // POST: api/Persons
        [HttpPost]
        public async Task<ActionResult<Persons>> PostPerson(PersonsDto personsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = _mapper.Map<Persons>(personsDto);

            var bar = await _context.bars
                .Include(b => b.Beer) 
                .FirstOrDefaultAsync(b => b.Id == personsDto.BarId);

            if (bar == null)
            {
                return NotFound("Bar is not found!");
            }

            person.Bar = bar;

            _context.persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // PUT: api/Persons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, [FromBody] Persons person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.persons.Any(e => e.Id == id);
        }
    }
}
