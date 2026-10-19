using Hangfire;
using Hirealdoor.Installer;
using Hirealdoor.Jobs;
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


app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hirealdoor Dashboard",
});

app.UseMiddleware<SimpleMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var timezone = new RecurringJobOptions
{
    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran")
};

RecurringJob.AddOrUpdate<EmailService>(
    "BirthdayEmail",
    "critical",
    x => x.SendVerificationEmailAsync("roshani.net","234567"),
    "0 13 * * *",
    timezone
);
RecurringJob.AddOrUpdate<IterativeJobs>("WriteHello","critical",j => j.WriteHello(),
    "* * * * *",
    timezone
);

app.Run();