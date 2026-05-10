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
        EnsureCountry(Constants.Countries.HU, "HU");
        SeedOlympics2026();
        SeedOlympics2026Matchups();
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

            var latvija = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.LV);

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.US).Id,
                GameTime = new DateTime(2026, 02, 12, 22, 10, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                GuestTeamId = latvija.Id,
                HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.DE).Id,
                GameTime = new DateTime(2026, 02, 14, 13, 10, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                GuestTeamId = latvija.Id,
                HomeTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.DK).Id,
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

            var latvija = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.LV);

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.FR).Id,
                GameTime = new DateTime(2025, 05, 10, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.CA).Id,
                GameTime = new DateTime(2025, 05, 11, 17, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.SI).Id,
                GameTime = new DateTime(2025, 05, 13, 17, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.SE).Id,
                GameTime = new DateTime(2025, 05, 14, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.FI).Id,
                GameTime = new DateTime(2025, 05, 17, 13, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.SK).Id,
                GameTime = new DateTime(2025, 05, 18, 21, 20, 00),
            });

            Matchups.Add(new Matchup
            {
                TournamentId = tournament.Id,
                HomeTeamId = latvija.Id,
                GuestTeamId = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.AT).Id,
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
            Countries.Add(new Country { Name = Constants.Countries.SI, FlagCode = "SI" });
            Countries.Add(new Country { Name = Constants.Countries.HU, FlagCode = "HU" });

            SaveChanges();
        }
    }

    private void EnsureCountry(string name, string flagCode)
    {
        if (!Countries.Any(c => c.Name == name))
        {
            Countries.Add(new Country { Name = name, FlagCode = flagCode });
            SaveChanges();
        }
    }

    private void SeedWC2026()
    {
        if (!Tournaments.Any(_ => _.Name == "IIHF 2026"))
        {
            var hostCountry = Countries.FirstOrDefault(_ => _.Name == Constants.Countries.CH);
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

        Game(C.FI, C.DE, new DateTime(2026, 5, 15, 17, 20, 0));
        Game(C.CA, C.SE, new DateTime(2026, 5, 15, 17, 20, 0));
        Game(C.US, C.CH, new DateTime(2026, 5, 15, 21, 20, 0));
        Game(C.CZ, C.DK, new DateTime(2026, 5, 15, 21, 20, 0));

        Game(C.GB, C.AT, new DateTime(2026, 5, 16, 13, 20, 0));
        Game(C.SK, C.NO, new DateTime(2026, 5, 16, 13, 20, 0));
        Game(C.HU, C.FI, new DateTime(2026, 5, 16, 17, 20, 0));
        Game(C.IT, C.CA, new DateTime(2026, 5, 16, 17, 20, 0));
        Game(C.CH, C.LV, new DateTime(2026, 5, 16, 21, 20, 0));
        Game(C.SI, C.CZ, new DateTime(2026, 5, 16, 21, 20, 0));

        Game(C.GB, C.US, new DateTime(2026, 5, 17, 13, 20, 0));
        Game(C.IT, C.SK, new DateTime(2026, 5, 17, 13, 20, 0));
        Game(C.AT, C.HU, new DateTime(2026, 5, 17, 17, 20, 0));
        Game(C.DK, C.SE, new DateTime(2026, 5, 17, 17, 20, 0));
        Game(C.DE, C.LV, new DateTime(2026, 5, 17, 21, 20, 0));
        Game(C.NO, C.SI, new DateTime(2026, 5, 17, 21, 20, 0));

        Game(C.FI, C.US, new DateTime(2026, 5, 18, 17, 20, 0));
        Game(C.CA, C.DK, new DateTime(2026, 5, 18, 17, 20, 0));
        Game(C.DE, C.CH, new DateTime(2026, 5, 18, 21, 20, 0));
        Game(C.SE, C.CZ, new DateTime(2026, 5, 18, 21, 20, 0));

        Game(C.LV, C.AT, new DateTime(2026, 5, 19, 17, 20, 0));
        Game(C.IT, C.NO, new DateTime(2026, 5, 19, 17, 20, 0));
        Game(C.HU, C.GB, new DateTime(2026, 5, 19, 21, 20, 0));
        Game(C.SI, C.SK, new DateTime(2026, 5, 19, 21, 20, 0));

        Game(C.AT, C.CH, new DateTime(2026, 5, 20, 17, 20, 0));
        Game(C.CZ, C.IT, new DateTime(2026, 5, 20, 17, 20, 0));
        Game(C.US, C.DE, new DateTime(2026, 5, 20, 21, 20, 0));
        Game(C.SE, C.SI, new DateTime(2026, 5, 20, 21, 20, 0));

        Game(C.LV, C.FI, new DateTime(2026, 5, 21, 17, 20, 0));
        Game(C.CA, C.NO, new DateTime(2026, 5, 21, 17, 20, 0));
        Game(C.CH, C.GB, new DateTime(2026, 5, 21, 21, 20, 0));
        Game(C.DK, C.SK, new DateTime(2026, 5, 21, 21, 20, 0));

        Game(C.DE, C.HU, new DateTime(2026, 5, 22, 17, 20, 0));
        Game(C.CA, C.SI, new DateTime(2026, 5, 22, 17, 20, 0));
        Game(C.FI, C.GB, new DateTime(2026, 5, 22, 21, 20, 0));
        Game(C.SE, C.IT, new DateTime(2026, 5, 22, 21, 20, 0));

        Game(C.LV, C.US, new DateTime(2026, 5, 23, 13, 20, 0));
        Game(C.DK, C.SI, new DateTime(2026, 5, 23, 13, 20, 0));
        Game(C.CH, C.HU, new DateTime(2026, 5, 23, 17, 20, 0));
        Game(C.SK, C.CZ, new DateTime(2026, 5, 23, 17, 20, 0));
        Game(C.AT, C.DE, new DateTime(2026, 5, 23, 21, 20, 0));
        Game(C.NO, C.SE, new DateTime(2026, 5, 23, 21, 20, 0));

        Game(C.GB, C.LV, new DateTime(2026, 5, 24, 17, 20, 0));
        Game(C.DK, C.IT, new DateTime(2026, 5, 24, 17, 20, 0));
        Game(C.FI, C.AT, new DateTime(2026, 5, 24, 21, 20, 0));
        Game(C.SK, C.CA, new DateTime(2026, 5, 24, 21, 20, 0));

        Game(C.US, C.HU, new DateTime(2026, 5, 25, 17, 20, 0));
        Game(C.CZ, C.NO, new DateTime(2026, 5, 25, 17, 20, 0));
        Game(C.DE, C.GB, new DateTime(2026, 5, 25, 21, 20, 0));
        Game(C.SI, C.IT, new DateTime(2026, 5, 25, 21, 20, 0));

        Game(C.HU, C.LV, new DateTime(2026, 5, 26, 13, 20, 0));
        Game(C.NO, C.DK, new DateTime(2026, 5, 26, 13, 20, 0));
        Game(C.US, C.AT, new DateTime(2026, 5, 26, 17, 20, 0));
        Game(C.SE, C.SK, new DateTime(2026, 5, 26, 17, 20, 0));
        Game(C.CH, C.FI, new DateTime(2026, 5, 26, 21, 20, 0));
        Game(C.CZ, C.CA, new DateTime(2026, 5, 26, 21, 20, 0));

        SaveChanges();
    }
}
