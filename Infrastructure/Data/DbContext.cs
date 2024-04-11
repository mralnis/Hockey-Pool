using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            if (!this.Countries.Any())
            {
                this.Countries.Add(new Country { Name = "Latvija", FlagCode = "LV" });
                this.Countries.Add(new Country { Name = "ASV", FlagCode = "US" });
                this.Countries.Add(new Country { Name = "Zviedrija", FlagCode = "SE" });
                this.Countries.Add(new Country { Name = "Kanâda", FlagCode = "CA" });
                this.Countries.Add(new Country { Name = "Kazahstâna", FlagCode = "KZ" });
                this.Countries.Add(new Country { Name = "Itâlija", FlagCode = "IT" });
                this.Countries.Add(new Country { Name = "Norvçìija", FlagCode = "NO" });
                this.Countries.Add(new Country { Name = "Somija", FlagCode = "FI" });
                this.Countries.Add(new Country { Name = "Vâcija", FlagCode = "DE" });
                this.Countries.Add(new Country { Name = "Èehija", FlagCode = "CZ" });
                this.Countries.Add(new Country { Name = "Lielbritânija", FlagCode = "GB" });
                this.Countries.Add(new Country { Name = "Austrija", FlagCode = "AT" });
                this.Countries.Add(new Country { Name = "Ðveice", FlagCode = "CH" });
                this.Countries.Add(new Country { Name = "Dânija", FlagCode = "DK" });
                this.Countries.Add(new Country { Name = "Francija", FlagCode = "FR" });
                this.Countries.Add(new Country { Name = "Slovâkija", FlagCode = "SK" });
                this.Countries.Add(new Country { Name = "Polija", FlagCode = "PL" });

                this.SaveChanges();

            }
        }
    }
}
