using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using C = HockeyPool.Infrastructure.Constants.Countries;

namespace HockeyPool.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Matchup> Matchups { get; set; }
    public DbSet<PlayerTournament> PlayerTournaments { get; set; }
    public DbSet<Prediction> Predictions { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<PredictionLog> PredictionLogs { get; set; }
    public DbSet<TournamentWinner> TournamentWinners { get; set; }

    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        SeedCountrys();
        SeedWC2026();
        SeedWC2026Matchups();
        await SeedRolesAsync(serviceProvider);
        await SeedAdminAsync(serviceProvider);
    }

    private async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        if (!Users.Any())
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin",
                Email = "admin",
                NormalizedUserName = "ADMIN"
            }, "admin");

            var user = await userManager.FindByNameAsync("admin");

            await userManager.AddToRoleAsync(user, "Admin");
            SaveChanges();
        }
    }

    private async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string roleName = "Admin";

        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Create the role
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private void SeedOlympics2026Matchups()
    {
        if (!Matchups.Any())
        {
            var tournament = Tournaments.FirstOrDefault(_ => _.Name == "Olympics Milano Cortina 2026");

            var latvija = Countries.FirstOrDefault(_ => _.Name == C.LV);

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.US).Id,
                GameTime = new DateTime(2026, 02, 12, 22, 10, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                GuestTeamId = latvija.Id,
                HomeTeamId = Countries.FirstOrDefault(_ => _.Name == C.DE).Id,
                GameTime = new DateTime(2026, 02, 14, 13, 10, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                GuestTeamId = latvija.Id,
                HomeTeamId = Countries.FirstOrDefault(_ => _.Name == C.DK).Id,
                GameTime = new DateTime(2026, 02, 15, 20, 10, 00),
            });


            SaveChanges();
        }
    }

    private void SeedOlympics2026()
    {
        if (!Tournaments.Any(_ => _.Name == "Olympics Milano Cortina 2026"))
        {
            Tournaments.Add(new Tournament
            {
                CountryId = 6,
                StartDate = new DateTime(2026, 02, 06),
                EndDate = new DateTime(2026, 02, 22),
                MatchupClosingTime = 15,
                Name = "Olympics Milano Cortina 2026",
                IsActive = true,
                PointsForPerfect = 4,
                PointForDifference = 2,
                PointsForWinnerOnly = 1
            });

            SaveChanges();
        }
    }

    private void Seed2025Matchups()
    {
        if (!Matchups.Any())
        {
            var tournament = Tournaments.FirstOrDefault(_ => _.Name == "IIHF 2025");

            var latvija = Countries.FirstOrDefault(_ => _.Name == C.LV);

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.FR).Id,
                GameTime = new DateTime(2025, 05, 10, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.CA).Id,
                GameTime = new DateTime(2025, 05, 11, 17, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.SI).Id,
                GameTime = new DateTime(2025, 05, 13, 17, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.SE).Id,
                GameTime = new DateTime(2025, 05, 14, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.FI).Id,
                GameTime = new DateTime(2025, 05, 17, 13, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.SK).Id,
                GameTime = new DateTime(2025, 05, 18, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == C.AT).Id,
                GameTime = new DateTime(2025, 05, 20, 13, 20, 00),
            });

            SaveChanges();
        }
    }

    private void Seed2025Tournament()
    {
        if (!Tournaments.Any(_ => _.Name == "IIHF 2025"))
        {
            Tournaments.Add(new Tournament
            {
                CountryId = 10,
                StartDate = new DateTime(2025, 05, 09),
                EndDate = new DateTime(2025, 05, 25),
                MatchupClosingTime = 15,
                Name = "IIHF 2025",
                IsActive = false,
                PointsForPerfect = 4,
                PointForDifference = 2,
                PointsForWinnerOnly = 1
            });

            SaveChanges();
        }
    }

    private void SeedCountrys()
    {
        if (!Countries.Any())
        {
            Countries.Add(new Country { Name = C.LV, FlagCode = "LV" });
            Countries.Add(new Country { Name = C.US, FlagCode = "US" });
            Countries.Add(new Country { Name = C.SE, FlagCode = "SE" });
            Countries.Add(new Country { Name = C.CA, FlagCode = "CA" });
            Countries.Add(new Country { Name = C.KZ, FlagCode = "KZ" });
            Countries.Add(new Country { Name = C.IT, FlagCode = "IT" });
            Countries.Add(new Country { Name = C.NO, FlagCode = "NO" });
            Countries.Add(new Country { Name = C.FI, FlagCode = "FI" });
            Countries.Add(new Country { Name = C.DE, FlagCode = "DE" });
            Countries.Add(new Country { Name = C.CZ, FlagCode = "CZ" });
            Countries.Add(new Country { Name = C.GB, FlagCode = "GB" });
            Countries.Add(new Country { Name = C.AT, FlagCode = "AT" });
            Countries.Add(new Country { Name = C.CH, FlagCode = "CH" });
            Countries.Add(new Country { Name = C.DK, FlagCode = "DK" });
            Countries.Add(new Country { Name = C.FR, FlagCode = "FR" });
            Countries.Add(new Country { Name = C.SK, FlagCode = "SK" });
            Countries.Add(new Country { Name = C.PL, FlagCode = "PL" });
            Countries.Add(new Country { Name = C.SI, FlagCode = "SI" });
            Countries.Add(new Country { Name = C.HU, FlagCode = "HU" });

            SaveChanges();
        }
    }
    private void SeedWC2026()
    {
        if (!Tournaments.Any(_ => _.Name == "IIHF 2026"))
        {
            var hostCountry = Countries.FirstOrDefault(_ => _.Name == C.CH);
            Tournaments.Add(new Tournament
            {
                CountryId = hostCountry?.Id ?? 13,
                StartDate = new DateTime(2026, 05, 15),
                EndDate = new DateTime(2026, 05, 31),
                MatchupClosingTime = 15,
                Name = "IIHF 2026",
                IsActive = false,
                PointsForPerfect = 4,
                PointForDifference = 2,
                PointsForWinnerOnly = 1
            });

            SaveChanges();
        }
    }

    private void SeedWC2026Matchups()
    {
        var tournament = Tournaments.FirstOrDefault(_ => _.Name == "IIHF 2026");
        if (tournament == null) return;
        if (Matchups.Any(m => m.TournamentId == tournament.Id)) return;

        int Id(string name) => Countries.First(c => c.Name == name).Id;
        void Game(string home, string guest, DateTime time) =>
            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = Id(home),
                GuestTeamId = Id(guest),
                GameTime = time,
            });


        Game(C.CH, C.LV, new DateTime(2026, 5, 16, 21, 20, 0));        
        Game(C.DE, C.LV, new DateTime(2026, 5, 17, 21, 20, 0)); 
        Game(C.LV, C.AT, new DateTime(2026, 5, 19, 17, 20, 0));
        Game(C.LV, C.FI, new DateTime(2026, 5, 21, 17, 20, 0));
        Game(C.LV, C.US, new DateTime(2026, 5, 23, 13, 20, 0));
        Game(C.GB, C.LV, new DateTime(2026, 5, 24, 17, 20, 0));
        Game(C.HU, C.LV, new DateTime(2026, 5, 26, 13, 20, 0));


        SaveChanges();
    }
}
