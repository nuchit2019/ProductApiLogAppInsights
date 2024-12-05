using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Repositories;
using ProductApi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Read Serilog configuration from `appsettings.json`
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Requires appsettings.json configuration
    .WriteTo.Console() // Optional: Log to the console
    .CreateLogger();

// Assign Serilog as the logging provider
builder.Logging.AddSerilog(logger);


// Add Application Insights Telemetry
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Add in-memory database
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ProductDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
