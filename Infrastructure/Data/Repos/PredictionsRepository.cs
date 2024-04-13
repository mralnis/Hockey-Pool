using HockeyPool.Infrastructure.Data.Models;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class PredictionsRepository : Repository<Prediction>
    {
        public PredictionsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

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
