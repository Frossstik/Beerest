using AutoMapper;
using Beerest;
using Beerest.gRPC;
using Beerest.Interfaces;
using Beerest.Mapping.DTO;
using Beerest.Models;
using Beerest.RabbitMQ;
using CheckService;
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
    private readonly CheckServiceClient _checkServiceClient;
    private readonly RabbitMqPublisher _rabbitMqPublisher;

    public BeersController(
        IBeersRepository repository, 
        IMapper mapper, 
        AppDbContext context, 
        RabbitMqPublisher rabbitMqPublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _checkServiceClient = new CheckServiceClient("http://localhost:5100");
        _context = context;
        _rabbitMqPublisher = rabbitMqPublisher;
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

    [HttpPost("create-check")]
    public async Task<IActionResult> CreateCheck([FromBody] CreateCheckMessage requestMessage)
    {
        var grpcRequest = _mapper.Map<CreateCheckRequest>(requestMessage);

        var response = await _checkServiceClient.CreateCheckAsync(grpcRequest);

        return Ok(new { FilePath = response.FilePath });
    }

    [HttpPost("create-check-async")]
    public async Task<IActionResult> CreateCheckAsync([FromBody] CreateCheckMessage requestMessage)
    {
        var request = _mapper.Map<CreateCheckRequest>(requestMessage);

        await _rabbitMqPublisher.PublishMessageAsync(request);

        return Accepted(new { Message = "Check creation request has been queued" });
    }

}