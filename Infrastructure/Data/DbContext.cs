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
                this.Countries.Add(new Country { Name = "Kan�da", FlagCode = "CA" });
                this.Countries.Add(new Country { Name = "Kazahst�na", FlagCode = "KZ" });
                this.Countries.Add(new Country { Name = "It�lija", FlagCode = "IT" });
                this.Countries.Add(new Country { Name = "Norv��ija", FlagCode = "NO" });
                this.Countries.Add(new Country { Name = "Somija", FlagCode = "FI" });
                this.Countries.Add(new Country { Name = "V�cija", FlagCode = "DE" });
                this.Countries.Add(new Country { Name = "�ehija", FlagCode = "CZ" });
                this.Countries.Add(new Country { Name = "Lielbrit�nija", FlagCode = "GB" });
                this.Countries.Add(new Country { Name = "Austrija", FlagCode = "AT" });
                this.Countries.Add(new Country { Name = "�veice", FlagCode = "CH" });
                this.Countries.Add(new Country { Name = "D�nija", FlagCode = "DK" });
                this.Countries.Add(new Country { Name = "Francija", FlagCode = "FR" });
                this.Countries.Add(new Country { Name = "Slov�kija", FlagCode = "SK" });
                this.Countries.Add(new Country { Name = "Polija", FlagCode = "PL" });

                this.SaveChanges();

            }
        }
    }
}
