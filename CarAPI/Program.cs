using CarAPI.Data;
using CarAPI.Service;

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
// register CarMakeService as scoped if retrive any data when initialize object
builder.Services.AddSingleton<CarMakeService>();
builder.Services.AddHttpClient();

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
