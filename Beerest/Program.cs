using Beerest;
using Beerest.GraphQL.Mutations;
using Beerest.GraphQL.Queries;
using Beerest.Interfaces;
using Beerest.Mapping;
using Beerest.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

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
