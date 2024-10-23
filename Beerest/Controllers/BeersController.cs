using AutoMapper;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using Beerest.Models;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BeersController : ControllerBase
{
    private readonly IBeersRepository _repository;
    private readonly IMapper _mapper;

    public BeersController(IBeersRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<Beers>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Beers?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] BeersDto beersDto)
    {
        var beer = _mapper.Map<Beers>(beersDto);

        await _repository.CreateAsync(beer);
        return Ok(beer);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(int id, [FromBody] Beers beer)
    {
        if (id != beer.Id) return;
        await _repository.UpdateAsync(beer);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}