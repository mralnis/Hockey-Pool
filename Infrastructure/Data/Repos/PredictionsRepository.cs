using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data.Repos;

public class PredictionsRepository : GenericRepository<Prediction>
{
    public PredictionsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public override async Task AddAsync(Prediction entity)
    {
        AddPredictionLog(entity, string.Empty, string.Empty);
        await base.AddAsync(entity);
    }
    public async Task AddAsync(Prediction entity, string ipAddress, string browserVersion)
    {
        AddPredictionLog(entity, ipAddress, browserVersion);
        await base.AddAsync(entity);
    }
    public async Task UpdateAsync(Prediction entity, string ipAddress, string browserVersion)
    {
        AddPredictionLog(entity, ipAddress, browserVersion);
        await base.UpdateAsync(entity);
    }
    public override async Task UpdateAsync(Prediction entity)
    {
        AddPredictionLog(entity, string.Empty, string.Empty);
        await base.UpdateAsync(entity);
    }
    public override async Task AddRange(IEnumerable<Prediction> entities)
    {
        foreach(var entity in entities)
        {
            AddPredictionLog(entity, string.Empty, string.Empty);
        }

        await base.AddRange(entities);
    }

    private void AddPredictionLog(Prediction entity, string ipAddress, string browserVersion)
    {
        _dbContext.PredictionLogs.Add(new PredictionLog
        {
            AspNetUserId = entity.AspNetUserId,
            GuestTeamScore = entity.GuestTeamScore,
            HomeTeamScore = entity.HomeTeamScore,
            MatchupId = entity.MatchupId,
            TimeStamp = DateTime.Now,
            IpAddress = ipAddress,
            BrowserVersion = browserVersion
        });
    }

    public async Task ClearPredictionAsync(int predictionId)
    {
        var prediction = await _dbContext.Predictions.FindAsync(predictionId);
        if (prediction == null) return;

        prediction.HomeTeamScore = null;
        prediction.GuestTeamScore = null;
        prediction.PointsEarned = null;

        AddPredictionLog(prediction, string.Empty, string.Empty);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Prediction>> GetPredictionsByMatchupIdAsync(int matchupId)
    {
        return await _dbContext.Predictions
            .Where(x => x.MatchupId == matchupId && x.HomeTeamScore != null)
            .ToListAsync();
    }

    public async Task<List<Prediction>> GetAllPredictionsAsync()
    {
        return await _dbContext.Predictions.ToListAsync();
    }
    
    public async Task<List<Prediction>> GetUserPredictionsAsync(Guid userId)
    {
        return await _dbContext.Predictions.Where(_ => _.AspNetUserId == userId).ToListAsync();
    }
}
