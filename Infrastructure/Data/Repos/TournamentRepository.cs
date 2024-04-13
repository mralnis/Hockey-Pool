using HockeyPool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class TournamentRepository : Repository<Tournament>
    {
        public TournamentRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Tournament> GetActiveTournamentAsync()
        {
            try
            {
                var activeTournament = await _dbContext.Tournaments.Where(_ => _.IsActive).FirstOrDefaultAsync();

                if (activeTournament == null)
                {
                    // how will you later know if you found an actual tournament or got a blank one?
                    activeTournament = new Tournament();
                }

                return activeTournament;
            }
            catch (Exception ex)
            {
                //why catch if you wont do anything with it?
                throw;
            }
        }

        public async Task<List<Tournament>> GetListByMultipleIdsAsync(IEnumerable<int> ids)
        {
            var result = await _dbContext.Tournaments.Where(x => ids.Contains(x.Id)).ToListAsync();
            return result;
        }
    }
}
