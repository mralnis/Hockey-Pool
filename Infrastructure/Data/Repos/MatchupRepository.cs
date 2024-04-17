using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class MatchupRepository : Repository<Matchup>
    {
        private readonly TournamentRepository _tournamentRepository;

        public MatchupRepository(ApplicationDbContext dbContext, TournamentRepository tournamentRepository) : base(dbContext) 
        {
            _tournamentRepository = tournamentRepository;
        }


        public async Task<Matchup> Create(Matchup matchup)
        {
            var result = await _dbContext.Matchups.AddAsync(matchup);
            await AddMatchupToExistingPlayerPredictions(matchup);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async void Edit(Matchup matchup)
        {
            _dbContext.Entry(matchup).State = EntityState.Modified;
            if (matchup.HomeTeamScore != null)
            {
                SetScore(matchup);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Matchup>> GetActiveMatchups()
        {
            var activeTournament = await _tournamentRepository.GetActiveTournamentAsync();
            var result = await _dbContext.Matchups.Where(x=>x.TournamentId == activeTournament.Id).ToListAsync();
            return result;
        }

        public async Task<Matchup?> GetClosestMatchup()
        {
            var result = await _dbContext.Matchups.Where(x => x.HomeTeamScore == null).OrderBy(x => x.GameTime).FirstOrDefaultAsync();
            return result;
        }

        public async void Delete(int matchupId)
        {
            var matchup = await _dbContext.Matchups.FindAsync(matchupId);
            if (matchup == null)
            {
                throw new Exception($"Failed to find matchup with id:{matchupId} when deleting");
            }
            _dbContext.Matchups.Remove(matchup);
            await _dbContext.SaveChangesAsync();
        }

        private async Task AddMatchupToExistingPlayerPredictions(Matchup matchup)
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
                if (prediction.HomeTeamScore == null)
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
