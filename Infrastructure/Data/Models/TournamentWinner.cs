namespace HockeyPool.Infrastructure.Data.Models;

public class TournamentWinner
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public string AspNetUserId { get; set; }
    public int Place { get; set; } // 1, 2, or 3
}
