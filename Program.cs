using Hirealdoor.Models;
using Hirealdoor.Repositories;
using Hirealdoor.Repositories.Repository;
using Hirealdoor.Services;
using Hirealdoor.Services.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
// connecting DB
var myConnection= builder.Configuration.GetConnectionString("MySqlConnection")  ?? throw new InvalidOperationException(
        "Connection string 'MySqlConnection' was not found.");
builder.Services.AddDbContext<SqlContext>(opts=>opts.UseMySQL(myConnection));

//install IRepository and Repository as services
builder.Services.AddScoped<IOfficeRepository,OfficeRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IPersonRepository,PersonRepository>();
//install IService and Service as services
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
