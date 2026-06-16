using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TravelLogger;
using AutoMapper;
using TravelLogger.Models;
using TravelLogger.DTOs;
using AutoMapper.QueryableExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddAutoMapper(config => config.AddProfile<AutoMapperProfiles>());

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TravelLoggerDbContext>(builder.Configuration["TravelLoggerDbConnectionString"]);

var app = builder.Build();

// Comment out HTTPS redirection for now to simplify testing
// app.UseHttpsRedirection();

// Configure Swagger for all environments during development
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

// Add all endpoints here

app.MapPost("/api/users", (TravelLoggerDbContext db, IMapper mapper, UserDto userDto) =>
{
    User user = mapper.Map<User>(userDto);

    db.Users.Add(user);
    db.SaveChanges();

    UserDto created = mapper.Map<UserDto>(user);
    return Results.Created($"/api/user/{created.Id}", created);
});

app.MapGet("/api/users/signin/{email}", (TravelLoggerDbContext db, IMapper mapper, string? email) =>
{
    UserDto? user = db.Users
        .Where(u => u.Email == email)
        .ProjectTo<UserDto>(mapper.ConfigurationProvider)
        .SingleOrDefault();

    return user != null ? Results.Ok(user) : Results.NotFound();
});

app.MapGet("/api/users/{id}", (TravelLoggerDbContext db, IMapper mapper, int id) =>
{
    UserDto? user = db.Users
        .ProjectTo<UserDto>(mapper.ConfigurationProvider)
        .SingleOrDefault(u => u.Id == id);

    return user != null ? Results.Ok(user) : Results.NotFound();
});

app.MapPut("/api/users/{id}", (TravelLoggerDbContext db, IMapper mapper, int id, UserDto updateDto) =>
{
    User? user = db.Users.SingleOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound();
    }

    user.Email = updateDto.Email;
    user.Description = updateDto.Description;
    user.ImageUrl = updateDto.ImageUrl;

    db.SaveChanges();

    return Results.NoContent();
});

app.MapGet("/api/cities/{cityId}/users", (TravelLoggerDbContext db, IMapper mapper, int cityId) =>
{
    List<UserDto> users = db.Users
        .Where(u => u.Logs
            .OrderByDescending(l => l.Date)
            .Select(l => l.CityId)
            .FirstOrDefault() == cityId)
        .ProjectTo<UserDto>(mapper.ConfigurationProvider)
        .ToList();

    return Results.Ok(users);
});

app.Run();