using Beerest;
using Beerest.GraphQL.Mutations;
using Beerest.GraphQL.Queries;
using Beerest.Interfaces;
using Beerest.Mapping;
using Beerest.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(endpointOptions =>
    {
        endpointOptions.Protocols = HttpProtocols.Http2;
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(AppRepository<>));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddScoped(typeof(IBeersRepository), typeof(BeersRepository));
builder.Services.AddScoped(typeof(IBarsRepository), typeof(BarsRepository));
builder.Services.AddScoped(typeof(IPersonsRepository), typeof(PersonsRepository));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddGraphQLServer()
        .AddQueryType<AppQuery>()
        .AddMutationType<AppMutation>();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console());

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("user");
            h.Password("rabbitmq");
            h.Heartbeat(30);
      
        });

        cfg.ConfigureEndpoints(context);
    });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGraphQL();

app.Run();
