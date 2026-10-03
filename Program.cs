using Hirealdoor.Installer;
using Hirealdoor.Models;
using Hirealdoor.Repositories;
using Hirealdoor.Repositories.Repository;
using Hirealdoor.Services;
using Hirealdoor.Services.Service;
using Hirealdoor.Setting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
// connecting DB
builder.Services.InstallServicesInAssembly(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<SimpleMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();