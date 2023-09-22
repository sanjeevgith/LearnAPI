using AutoMapper;
using LearnAPI.Container;
using LearnAPI.Helper;
using LearnAPI.Repos;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection 
builder.Services.AddTransient<ICustomerService , CustomerService>();
//end DE


//connection string
builder.Services.AddDbContext<LearndataContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("apicon")));
//end con string

//handler service
var automapper = new MapperConfiguration(item=>item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);
//end handler service


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
