namespace HockeyPool.Infrastructure.Data.Models
{
    public class PlayerTournament
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Guid ApsnetUserId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
