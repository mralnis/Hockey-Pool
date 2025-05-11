using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HockeyPool.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Matchup> Matchups { get; set; }
    public DbSet<PlayerTournament> PlayerTournaments { get; set; }
    public DbSet<Prediction> Predictions { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<PredictionLog> PredictionLogs { get; set; }

    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        SeedCountrys();
        Seed2025Tournament();
        Seed2025Matchups();
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
                IsActive = true,
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

            SaveChanges();
        }
    }
}
