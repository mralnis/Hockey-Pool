using System.ComponentModel.DataAnnotations.Schema;

namespace HockeyPool.Infrastructure.Data.Models
{
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        public Guid AspNetUserId { get; set; }
        public int? HomeTeamScore { get; set; }

        public int? EnemyTeamScore { get; set; }

        public int? PointsEarned { get; set; }
        public Matchup Matchup { get; set; }

        [NotMapped]
        public string EnemyTeamScoreText { get; set; }
        [NotMapped]
        public string HomeTeamScoreText { get; set; }
    }
}
