using HockeyPool.Infrastructure.Data.Models;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class PredictionsRepository(ApplicationDbContext dbContext)
    {
        public List<Prediction> GetAllPredictions()
        {
            return dbContext.Predictions.ToList();
        }
        public List<Prediction> GetUserPredictions(Guid userId)
        {
            return dbContext.Predictions.Where(_ => _.AspNetUserId == userId).ToList();
        }
    }
}
