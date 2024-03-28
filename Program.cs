using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// to register a DbContext (DataContext) and configure the database connection
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure CORS middleware to allow requests from origin "http://localhost:4200"
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200"));

app.MapControllers();

app.Run();