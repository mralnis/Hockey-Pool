using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class PredictionsRepository : GenericRepository<Prediction>
    {
        public PredictionsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task AddAsync(Prediction entity)
        {
            _dbContext.PredictionLogs.Add(new PredictionLog
            {
                AspNetUserId = entity.AspNetUserId,
                GuestTeamScore = entity.GuestTeamScore,
                HomeTeamScore = entity.HomeTeamScore,
                MatchupId = entity.MatchupId,
            });

            await base.AddAsync(entity);
        }
        public override async Task UpdateAsync(Prediction entity)
        {
            _dbContext.PredictionLogs.Add(new PredictionLog
            {
                AspNetUserId = entity.AspNetUserId,
                GuestTeamScore = entity.GuestTeamScore,
                HomeTeamScore = entity.HomeTeamScore,
                MatchupId = entity.MatchupId,
            });
            await base.UpdateAsync(entity);
        }
        public override async Task AddRange(IEnumerable<Prediction> entities)
        {
            foreach(var entity in entities)
            {
                _dbContext.PredictionLogs.Add(new PredictionLog
                {
                    AspNetUserId = entity.AspNetUserId,
                    GuestTeamScore = entity.GuestTeamScore,
                    HomeTeamScore = entity.HomeTeamScore,
                    MatchupId = entity.MatchupId,
                });
            }

            await base.AddRange(entities);
        }

        public List<Prediction> GetAllPredictions()
        {
            return _dbContext.Predictions.ToList();
        }
        public List<Prediction> GetUserPredictions(Guid userId)
        {
            return _dbContext.Predictions.Where(_ => _.AspNetUserId == userId).ToList();
        }
    }
}
