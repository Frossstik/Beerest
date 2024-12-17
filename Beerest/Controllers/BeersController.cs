using AutoMapper;
using Beerest;
using Beerest.gRPC;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using Beerest.Models;
using CheckService;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Policy;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BeersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IBeersRepository _repository;
    private readonly IMapper _mapper;
    //private readonly CheckServicePublisher _publisher;
    private readonly CheckServiceClient _checkServiceClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public BeersController(IBeersRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint, AppDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        //_publisher = publisher;
        _publishEndpoint =  publishEndpoint;
        _checkServiceClient = new CheckServiceClient("http://localhost:5100");
        _context = context;
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

    [HttpPost("create-check-async")]
    public async Task<IActionResult> CreateCheckAsync([FromBody] CreateCheckMessage requestMessage)
    {
        var beers = _context.beers
            .Select(b => new CheckItemDto
            {
                Name = b.Name ?? "Неизвестное пиво",
                Price = b.Price
            })
            .ToList();

        if (!beers.Any())
        {
            return BadRequest("Пива нету!");
        }

        await _publishEndpoint.Publish(requestMessage, context =>
        {
            context.SetRoutingKey("create-check-routing-key");
        });


        var json = JsonSerializer.Serialize(requestMessage);
        Console.WriteLine($"Serialized message: {json}");


        return Ok(new { Message = "Ушло на шину!" });
    }




    [HttpPost("create-check")]
    public async Task<IActionResult> CreateCheck([FromBody] CreateCheckMessage requestMessage)
    {
        var grpcRequest = _mapper.Map<CreateCheckRequest>(requestMessage);

        var response = await _checkServiceClient.CreateCheckAsync(grpcRequest);

        return Ok(new { FilePath = response.FilePath });
    }

}