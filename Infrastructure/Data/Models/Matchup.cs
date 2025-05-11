namespace HockeyPool.Infrastructure.Data.Models;

public class Matchup
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public int HomeTeamId { get; set; }
    public int GuestTeamId { get; set; }
    public DateTime? GameTime { get; set; }  
    public int? HomeTeamScore { get; set; }
    public int? GuestTeamScore { get; set; }
}
