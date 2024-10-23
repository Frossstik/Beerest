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
using Beerest.Interfaces;
using Beerest.Repositories;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsRepository _repository;
        private readonly IBarsRepository _barsRepository;
        private readonly IMapper _mapper;

        public PersonsController(IPersonsRepository repository, IMapper mapper, IBarsRepository barsRepository)
        {
            _repository = repository;
            _barsRepository = barsRepository;
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

        [HttpGet]
        [Produces("application/hal+json")]
        public async Task<IActionResult> GetAllAsync(int index = 0, int count = 5)
        {
            var persons = await _repository.GetAllAsync();
            var total = persons.Count();
            var pagedPersons = persons.Skip(index).Take(count).ToList();

            var links = Paginate(Request.Path.ToString(), index, count, total);

            return Ok(new
            {
                total,
                count = pagedPersons.Count,
                index,
                items = pagedPersons.Select(p => _mapper.Map<Persons>(p)),
                _links = links
            });
        }

        [HttpGet("{id}")]
        public async Task<Persons?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PersonsDto personsDto)
        {
            var person = _mapper.Map<Persons>(personsDto);

            if (personsDto.BarId.HasValue)
            {
                var bar = await _barsRepository.GetByIdAsync(personsDto.BarId.Value);

                if (bar == null)
                {
                    return NotFound();
                }

                person.Bar = bar;
            }

            await _repository.CreateAsync(person);
            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PersonsDto personsDto)
        {
            var person = await _repository.GetByIdAsync(id);
            if (person == null) return NotFound();

            _mapper.Map(personsDto, person);
            await _repository.UpdateAsync(person);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var person = await _repository.GetByIdAsync(id);
            if (person == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}
