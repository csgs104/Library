using Microsoft.EntityFrameworkCore;
using LibraryData.Gateways.Abstract;
using LibraryData.Gateways;
using LibraryData.Data;
using LibraryData.Models;
using LibraryAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookGateway, BookGateway>();
builder.Services.AddScoped<IAuthorGateway, AuthorGateway>();

// builder.Services.AddDbContext<LibraryContext>();
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

await app.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

