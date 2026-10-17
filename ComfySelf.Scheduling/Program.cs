using System.Text;
using ComfySelf.Scheduling.Application.Clients;
using ComfySelf.Scheduling.Application.Clients.Abstractions;
using ComfySelf.Scheduling.Application.Repositories.Abstractions;
using ComfySelf.Scheduling.Application.Services;
using ComfySelf.Scheduling.Application.Services.Abstractions;
using ComfySelf.Scheduling.Infrastructure.Data;
using ComfySelf.Scheduling.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SchedulingDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SchedulingDatabase"));
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["AppSettings:Token"]!)),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var patientClientBaseUrl = builder.Configuration["Services:Patient:BaseUrl"]
    ?? throw new InvalidOperationException(
        "Patient API BaseUrl is not configured.");

builder.Services
    .AddHttpClient<IPatientClient, PatientClient>(client =>
    {
        //appsettings
        client.BaseAddress = new Uri(patientClientBaseUrl);
    })
    .AddStandardResilienceHandler(options =>
    {
        //TODO: Add more for resilience

        options.TotalRequestTimeout.Timeout =
            TimeSpan.FromSeconds(8);

        options.AttemptTimeout.Timeout =
            TimeSpan.FromSeconds(3);
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
