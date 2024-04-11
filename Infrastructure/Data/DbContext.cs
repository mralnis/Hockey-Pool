using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Matchup> Matchups { get; set; }
        public DbSet<PlayerTournament> PlayerTournaments { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        public void Seed()
        {
            if (!Countries.Any())
            {
                Countries.Add(new Country { Name = "Latvija", FlagCode = "LV" });
                Countries.Add(new Country { Name = "ASV", FlagCode = "US" });
                Countries.Add(new Country { Name = "Zviedrija", FlagCode = "SE" });
                Countries.Add(new Country { Name = "Kan�da", FlagCode = "CA" });
                Countries.Add(new Country { Name = "Kazahst�na", FlagCode = "KZ" });
                Countries.Add(new Country { Name = "It�lija", FlagCode = "IT" });
                Countries.Add(new Country { Name = "Norv��ija", FlagCode = "NO" });
                Countries.Add(new Country { Name = "Somija", FlagCode = "FI" });
                Countries.Add(new Country { Name = "V�cija", FlagCode = "DE" });
                Countries.Add(new Country { Name = "�ehija", FlagCode = "CZ" });
                Countries.Add(new Country { Name = "Lielbrit�nija", FlagCode = "GB" });
                Countries.Add(new Country { Name = "Austrija", FlagCode = "AT" });
                Countries.Add(new Country { Name = "�veice", FlagCode = "CH" });
                Countries.Add(new Country { Name = "D�nija", FlagCode = "DK" });
                Countries.Add(new Country { Name = "Francija", FlagCode = "FR" });
                Countries.Add(new Country { Name = "Slov�kija", FlagCode = "SK" });
                Countries.Add(new Country { Name = "Polija", FlagCode = "PL" });

                SaveChanges();
            }
            if (!Tournaments.Any(_ => _.Name == "IIHF 2024"))
            {
                Tournaments.Add(new Tournament 
                {
                    CountryId = 10, 
                    StartDate = new DateTime(2024, 05, 10), 
                    EndDate = new DateTime(2024, 05, 27), 
                    MatchupClosingTime = 15, 
                    Name = "IIHF 2024", 
                    IsActive = true, 
                    PointsForPerfect = 5, 
                    PointForDifference = 3, 
                    PointsForWinnerOnly = 1 
                });

                SaveChanges();
            }
        }
    }
}
