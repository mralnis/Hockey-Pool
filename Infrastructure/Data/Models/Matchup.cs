using System.ComponentModel.DataAnnotations.Schema;

namespace HockeyPool.Infrastructure.Data.Models
{
    public class Matchup
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime? GameTime { get; set; }
        public int TournamentId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? EnemyTeamScore { get; set; }
        public Country Country { get; set; }
        public Tournament Tournament { get; set; }

        [NotMapped]
        public bool CanVote { get; set; }
    }
}
