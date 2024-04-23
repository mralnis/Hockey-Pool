namespace HockeyPool.Infrastructure.Data.Models
{
    public class PredictionLog
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now.ToUniversalTime();
        public Guid AspNetUserId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? GuestTeamScore { get; set; }
    }
}
