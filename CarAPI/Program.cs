using CarAPI.Data;
using CarAPI.Service;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load configuration
builder.Configuration.AddJsonFile("appsettings.json");

// Configure services
builder.Services.Configure<CarMakeSettings>(builder.Configuration.GetSection("CarMakeSettings"));
// I read CsvFilePath from app.setting because I think tha file name changed always 
// we can Use IOptionSnapshot if automaitc apply changes if change file name 
// now if you want change file name you should rerun application in server 
builder.Services.AddSingleton<CarMakeCsvLoader>(sp => new CarMakeCsvLoader(sp.GetRequiredService<IOptions<CarMakeSettings>>().Value.CsvFilePath));
builder.Services.AddSingleton<CarMakeCacheService>();
// register CarMakeService as scoped if retrive any data when initialize object
builder.Services.AddSingleton<CarMakeService>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // Add memory cache

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
