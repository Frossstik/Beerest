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
using Beerest.Interfaces;
using Beerest.Repositories;

namespace Beerest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BarsController : ControllerBase
    {
        private readonly IBarsRepository _repository;
        private readonly IBeersRepository _beersRepository;
        private readonly IMapper _mapper;

        public BarsController(IBarsRepository repository, IBeersRepository beersRepository, IMapper mapper)
        {
            _repository = repository;
            _beersRepository = beersRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Bars>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<Bars?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] BarsDto barsDto)
        {
            var bar = _mapper.Map<Bars>(barsDto);
            var beers = new List<Beers>();

            foreach (var beerId in barsDto.BeerIds)
            {
                var beer = await _beersRepository.GetByIdAsync(beerId);
                if (beer == null)
                {
                    return NotFound();
                }
                beers.Add(beer);
            }

            bar.Beers = beers;

            await _repository.CreateAsync(bar);
            return Ok(bar);
        }

        [HttpPut("{id}")]
        public async Task UpdateAsync(int id, [FromBody] Bars bar)
        {
            if (id != bar.Id) return;
            await _repository.UpdateAsync(bar);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
