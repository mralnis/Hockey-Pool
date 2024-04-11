using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class TournamentRepository
    {
        readonly private ApplicationDbContext _dbContext;
        public TournamentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tournament> GetActiveTournamentAsync()
        {
            try
            {
                var activeTournament = await _dbContext.Tournaments.Where(_ => _.IsActive).FirstOrDefaultAsync();

                if (activeTournament == null)
                {
                    activeTournament = new Tournament();
                }

                return activeTournament;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
