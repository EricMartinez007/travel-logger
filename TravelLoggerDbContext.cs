using Microsoft.EntityFrameworkCore;
using TravelLogger.Models;

namespace TravelLogger;

public class TravelLoggerDbContext : DbContext
{

    // Define tables here
    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<Upvote> Upvotes { get; set; }

    public TravelLoggerDbContext(DbContextOptions<TravelLoggerDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data here
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "luffy@strawhats.com",
                Description = "Captain of the Straw Hat Pirates, future King of the Pirates.",
                ImageUrl = "https://example.com/luffy.png"
            },
            new User
            {
                Id = 2,
                Email = "zoro@strawhats.com",
                Description = "Swordsman aiming to be the world's greatest. Has no sense of direction.",
                ImageUrl = "https://example.com/zoro.png"
            },
            new User
            {
                Id = 3,
                Email = "nami@strawhats.com",
                Description = "Navigator of the crew and master of weather and maps.",
                ImageUrl = "https://example.com/nami.png"
            }
        );

        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Water 7" },
            new City { Id = 2, Name = "Alabasta" },
            new City { Id = 3, Name = "Dressrosa" },
            new City { Id = 4, Name = "Wano" }
        );

        modelBuilder.Entity<Logs>().HasData(
            new Logs
            {
                Id = 1,
                UserId = 1,
                CityId = 4,
                Comments = "Wano was an incredible adventure. Onigashima will never be the same!",
                Date = new DateTime(2026, 1, 15)
            },
            new Logs
            {
                Id = 2,
                UserId = 2,
                CityId = 1,
                Comments = "Got lost in Water 7 for hours, but the shipwrights here are the best.",
                Date = new DateTime(2026, 2, 3)
            },
            new Logs
            {
                Id = 3,
                UserId = 3,
                CityId = 2,
                Comments = "Alabasta's deserts are brutal. Bring plenty of water!",
                Date = new DateTime(2026, 3, 21)
            }
        );

        modelBuilder.Entity<Recommendation>().HasData(
            new Recommendation
            {
                Id = 1,
                UserId = 1,
                CityId = 4,
                Description = "Try the meat in Wano's Flower Capital. Best food on the seas!"
            },
            new Recommendation
            {
                Id = 2,
                UserId = 3,
                CityId = 3,
                Description = "Visit the colosseum in Dressrosa, but watch out for the local schemes."
            }
        );

        modelBuilder.Entity<Upvote>().HasData(
            new Upvote { Id = 1, RecommendationId = 1, UserId = 2 },
            new Upvote { Id = 2, RecommendationId = 1, UserId = 3 },
            new Upvote { Id = 3, RecommendationId = 2, UserId = 1 }
        );
    }
}
