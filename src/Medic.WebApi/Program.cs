using Serilog;
using Serilog.Events;
using Medic.Service.Helpers;
using Medic.WebApi.Extensions;
using Medic.WebApi.Middlewares;
using Medic.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CustomServices

builder.Services.AddServices();

// JWT

builder.Services.AddJwt(builder.Configuration);

builder.Services.ConfigureSwagger();

// Logger(serilog)

/*
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/program.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .CreateLogger();*/

/*
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();*/

/*
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
*/

// Lowercase route

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

// Web root Path

PathHelper.WebRootPath = Path.GetFullPath("wwwroot");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();