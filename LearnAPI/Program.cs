using AutoMapper;
using LearnAPI.Container;
using LearnAPI.Helper;
using LearnAPI.Repos;
using LearnAPI.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;

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


//basic Auth
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
//end basic auth



//handler service
var automapper = new MapperConfiguration(item=>item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);
//end handler service

  
builder.Services.AddCors(p => p.AddPolicy("corspolicy",build =>
{
    build.WithOrigins("*").AllowAnyOrigin().AllowAnyMethod();
}));
//end cors



//rate limiter
builder.Services.AddRateLimiter(p => p.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
{
    options.Window =  TimeSpan.FromSeconds(10);
    options.PermitLimit = 1;
    options.QueueLimit = 1;
    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode=401);
//end rate limiter


//log setting
var logpath = builder.Configuration.GetSection("Logging:Logpath").Value;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logpath)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);
//end log 

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
