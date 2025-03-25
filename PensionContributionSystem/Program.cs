using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PensionContributionSystem.BackgroundJobs;
using PensionContributionSystem.Context;
using PensionContributionSystem.Repository.Implementation;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;
using PensionContributionSystem.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pension Contribution System API", Version = "v1" });
});

// Register repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();

// Register services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddScoped<IEmployerService, EmployerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register Hnagfire
builder.Services.AddScoped<HangfireJobs>();

// Database configuration
var appDbConnectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnections");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(appDbConnectionString));

// Hangfire configuration
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(hangfireConnectionString));
builder.Services.AddHangfireServer();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pension Contribution System API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Hangfire dashboard and jobs
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<HangfireJobs>(
    "ValidateContributions",
    x => x.ValidateContributions(),
    Cron.Daily);

app.MapControllers();
app.Run();