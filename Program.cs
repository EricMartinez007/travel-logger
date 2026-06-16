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
app.MapPost("/api/recommendations" , (TravelLoggerDbContext db, IMapper mapper, RecommendationDto newRecommendationDTO) =>
{
   var recommendation = mapper.Map<Recommendation>(newRecommendationDTO);
   db.Recommendations.Add(recommendation);
   db.SaveChanges();
   return Results.Created($"api/recommendations/{recommendation.Id}", mapper.Map<RecommendationDto>(recommendation)); 
});


app.MapPut("/api/recommendations/{id}", (TravelLoggerDbContext db, IMapper mapper, int id, RecommendationDto recommendationDTO) =>
{
    Recommendation recommendationToUpdate = db.Recommendations.SingleOrDefault(r => r.Id == id );
    if (recommendationToUpdate == null)
    {
        return Results.NotFound();
    }
    recommendationToUpdate.Description = recommendationDTO.Description;
    db.SaveChanges();
    return Results.NoContent();
});



app.MapDelete("/api/recommendations/{id}", (TravelLoggerDbContext db, IMapper mapper, int id) =>
{
    Recommendation recommendation = db.Recommendations.SingleOrDefault(p => p.Id == id);
    if (recommendation == null)
    {
        return Results.NotFound();
    }
    db.Remove(recommendation);
    db.SaveChanges();
    return Results.NoContent();
});


app.MapGet("/api/cities/{cityId}/recommendations}", (TravelLoggerDbContext db, IMapper mapper, int cityId) =>
{
    var recommendations = db.Recommendations
    .Where(r => r.CityId == cityId)
    .ToList();

    return Results.Ok(mapper.Map<List<RecommendationDto>>(recommendations));
});


app.MapGet("/api/recommendations/{id}", (TravelLoggerDbContext db , IMapper mapper, int id) =>
{
    var recommendation = db.Recommendations
    .SingleOrDefault(r => r.Id == id);

    if (recommendation == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(mapper.Map<RecommendationDto>(recommendation));
});



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

app.MapPost("/api/logs", (TravelLoggerDbContext db, IMapper mapper, Logs newLog) =>
{
    db.Logs.Add(newLog);
    db.SaveChanges();

    LogDto createdLog = db.Logs
        .ProjectTo<LogDto>(mapper.ConfigurationProvider)
        .SingleOrDefault(log => log.Id == newLog.Id);

    return Results.Created($"/api/logs/{newLog.Id}", createdLog);
});

app.MapPut("/api/logs/{id}", (TravelLoggerDbContext db, IMapper mapper, int id, Logs updatedLog) => 
{
    Logs existingLog = db.Logs
        .SingleOrDefault(log => log.Id == id);

    if(existingLog == null)
    {
        return Results.NotFound();
    }
    

    existingLog.UserId = updatedLog.UserId;
    existingLog.CityId = updatedLog.CityId;
    existingLog.Comments = updatedLog.Comments;
    existingLog.Date = updatedLog.Date;

    db.SaveChanges();

    LogDto updatedLogDto = db.Logs
        .ProjectTo<LogDto>(mapper.ConfigurationProvider)
        .SingleOrDefault(log => log.Id == id);

    return Results.Ok(updatedLogDto);

});


app.MapDelete("/api/logs/{id}", (TravelLoggerDbContext db, int id) =>
{
    Logs logToDelete = db.Logs
        .SingleOrDefault(log => log.Id == id);

    if (logToDelete == null)
    {
        return Results.NotFound();
    }

    db.Logs.Remove(logToDelete);
    db.SaveChanges();

    return Results.NoContent();
});

app.MapGet("/api/users/{userId}/logs", (IMapper mapper, TravelLoggerDbContext db, int userId) =>
{
    List<LogDto> userLogs = db.Logs
        .Where(log => log.UserId == userId)
        .ProjectTo<LogDto>(mapper.ConfigurationProvider)
        .ToList();

    return Results.Ok(userLogs);
});

app.MapGet("/api/cities/{cityId}/logs", (IMapper mapper, TravelLoggerDbContext db, int cityId) =>
{
    List<LogDto> cityLogs = db.Logs
        .Where(log => log.CityId == cityId)
        .ProjectTo<LogDto>(mapper.ConfigurationProvider)
        .ToList();

    return Results.Ok(cityLogs);
});

app.Run();