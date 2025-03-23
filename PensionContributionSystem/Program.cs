using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PensionContributionSystem.BackgroundJobs;
using PensionContributionSystem.Context;
using PensionContributionSystem.Repository.Implementation;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;
using PensionContributionSystem.Service.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Retrieve connection strings from appsettings.json
var appDbConnectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnections");

// Register repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();

// Register services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddScoped<IEmployerService, EmployerService>();

// Register NotificationService
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register HangfireJobs
builder.Services.AddScoped<HangfireJobs>();

// Register Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(hangfireConnectionString);
});

// Register Application DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(appDbConnectionString);
});


builder.Services.AddHangfireServer();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add controllers and API explorer for Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pension Contribution System API", Version = "v1" });
});

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pension Contribution System API V1");
    });
}

app.UseHttpsRedirection();

// Use Hangfire dashboard
app.UseHangfireDashboard();

// Schedule the job to run daily
RecurringJob.AddOrUpdate<HangfireJobs>(
    "ValidateContributions",
    x => x.ValidateContributions(),
    Cron.Daily);

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePages();

app.MapControllers();

app.Run();