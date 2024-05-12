using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class MatchupRepository : GenericRepository<Matchup>
    {
        private readonly TournamentRepository _tournamentRepository;

        public MatchupRepository(ApplicationDbContext dbContext, TournamentRepository tournamentRepository) : base(dbContext) 
        {
            _tournamentRepository = tournamentRepository;
        }


        public async Task<Matchup> CreateAsync(Matchup matchup)
        {
            var result = await _dbContext.Matchups.AddAsync(matchup);
            await AddMatchupToExistingPlayerPredictionsAsync(matchup);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public override async Task UpdateAsync(Matchup matchup)
        {
            _dbContext.Entry(matchup).State = EntityState.Modified;
            if (matchup.HomeTeamScore != null && matchup.GuestTeamScore != null)
            {
                SetScore(matchup);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Matchup>> GetActiveMatchupsAsync()
        {
            var activeTournament = await _tournamentRepository.GetActiveTournamentAsync();
            var result = await _dbContext.Matchups.Where(x=>x.TournamentId == activeTournament.Id).ToListAsync();
            return result;
        }

        public async Task<Matchup?> GetClosestMatchupAsync()
        {
            var result = await _dbContext.Matchups.Where(_ => _.GameTime.Value.AddHours(4) > DateTime.Now).OrderBy(x => x.GameTime).FirstOrDefaultAsync();
            return result;
        }

        public async Task DeleteAsync(int matchupId)
        {
            var matchup = await _dbContext.Matchups.FindAsync(matchupId);
            if (matchup == null)
            {
                throw new Exception($"Failed to find matchup with id:{matchupId} when deleting");
            }
            _dbContext.Matchups.Remove(matchup);
            await _dbContext.SaveChangesAsync();
        }

        private async Task AddMatchupToExistingPlayerPredictionsAsync(Matchup matchup)
        {
            var players = await _dbContext.PlayerTournaments.Where(x => x.TournamentId == matchup.TournamentId).ToListAsync();
            foreach (var player in players)
            {
                var prediction = new Prediction()
                {
                    AspNetUserId = player.ApsnetUserId,
                    MatchupId = matchup.Id,
                };
                await _dbContext.Predictions.AddAsync(prediction);
            }
        }
        private void SetScore(Matchup matchup)
        {
            var predictions = _dbContext.Predictions.Where(x => x.MatchupId == matchup.Id).ToList();
            var tournament = _dbContext.Tournaments.Find(matchup.TournamentId);
            if (tournament == null)
            {
                throw new Exception($"Tournament id {matchup.TournamentId} not found while setting score for a matchup");
            }

            foreach (var prediction in predictions)
            {
                if (prediction.HomeTeamScore == null || prediction.HomeTeamScore == prediction.GuestTeamScore)
                {
                    prediction.PointsEarned = 0;
                    continue;
                }

                if (prediction.HomeTeamScore == matchup.HomeTeamScore && prediction.GuestTeamScore == matchup.GuestTeamScore)
                {
                    prediction.PointsEarned = tournament.PointsForPerfect;
                }
                else
                {
                    if (prediction.HomeTeamScore - prediction.GuestTeamScore == matchup.HomeTeamScore - matchup.GuestTeamScore)
                    {
                        if (prediction.HomeTeamScore == prediction.GuestTeamScore)
                        {
                            prediction.PointsEarned = 0;
                        }
                        prediction.PointsEarned = tournament.PointForDifference;
                    }
                    else
                    {
                        if (prediction.HomeTeamScore > prediction.GuestTeamScore == matchup.HomeTeamScore > matchup.GuestTeamScore)
                        {
                            prediction.PointsEarned = tournament.PointsForWinnerOnly;
                        }
                        else
                        {
                            prediction.PointsEarned = 0;
                        }
                    }
                }
            }
        }

    }
}
