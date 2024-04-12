using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

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
            SeedCountrys();
            Seed2024Tournament();
            Seed2024Matchups();
        }

        private void Seed2024Matchups()
        {
            if (!Matchups.Any())
            {
                var tournament = Tournaments.FirstOrDefault(_ => _.Name == "IIHF 2024");

                var latvija = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.LV);

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.PL).Id,
                    GuestTeamId = latvija.Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 11, 17, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = latvija.Id,
                    GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.FR).Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 12, 17, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.KZ).Id,
                    GuestTeamId = latvija.Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 14, 17, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.DE).Id,
                    GuestTeamId = latvija.Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 15, 17, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = latvija.Id,
                    GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.SE).Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 18, 13, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.SK).Id,
                    GuestTeamId = latvija.Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 19, 21, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                Matchups.Add(new Matchup
                {
                    TournamentId = tournament.Id,
                    HomeTeamId = latvija.Id,
                    GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.US).Id,
                    GameTime = new DateTimeOffset(new DateTime(2024, 05, 21, 17, 20, 00), TimeSpan.FromHours(3)).UtcDateTime
                });

                SaveChanges();  
            }
        }

        private void Seed2024Tournament()
        {
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

        private void SeedCountrys()
        {
            if (!Countries.Any())
            {
                Countries.Add(new Country { Name = Constants.Countries.LV, FlagCode = "LV" });
                Countries.Add(new Country { Name = Constants.Countries.US, FlagCode = "US" });
                Countries.Add(new Country { Name = Constants.Countries.SE, FlagCode = "SE" });
                Countries.Add(new Country { Name = Constants.Countries.CA, FlagCode = "CA" });
                Countries.Add(new Country { Name = Constants.Countries.KZ, FlagCode = "KZ" });
                Countries.Add(new Country { Name = Constants.Countries.IT, FlagCode = "IT" });
                Countries.Add(new Country { Name = Constants.Countries.NO, FlagCode = "NO" });
                Countries.Add(new Country { Name = Constants.Countries.FI, FlagCode = "FI" });
                Countries.Add(new Country { Name = Constants.Countries.DE, FlagCode = "DE" });
                Countries.Add(new Country { Name = Constants.Countries.CZ, FlagCode = "CZ" });
                Countries.Add(new Country { Name = Constants.Countries.GB, FlagCode = "GB" });
                Countries.Add(new Country { Name = Constants.Countries.AT, FlagCode = "AT" });
                Countries.Add(new Country { Name = Constants.Countries.CH, FlagCode = "CH" });
                Countries.Add(new Country { Name = Constants.Countries.DK, FlagCode = "DK" });
                Countries.Add(new Country { Name = Constants.Countries.FR, FlagCode = "FR" });
                Countries.Add(new Country { Name = Constants.Countries.SK, FlagCode = "SK" });
                Countries.Add(new Country { Name = Constants.Countries.PL, FlagCode = "PL" });

                SaveChanges();
            }
        }
    }
}
