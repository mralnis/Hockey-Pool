using HockeyPool.Infrastructure.Data.Models;
using HockeyPool.Infrastructure.Data.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HockeyPool.Infrastructure.Data
{
    public class UnitOfWork(ApplicationDbContext dbContext)
    {
        private ApplicationDbContext context = dbContext;

        private GenericRepository<Country>? countryRepository;
        private GenericRepository<PredictionLog>? predictionLogRepository;
        private GenericRepository<ApplicationUser>? userRepository;
        private TournamentRepository? tournamentRepository;
        private PredictionsRepository? predictionsRepository;
        private MatchupRepository? matchupRepository;

        public GenericRepository<PredictionLog> PredictionLogRepository
        {
            get
            {
                if (this.predictionLogRepository == null)
                {
                    this.predictionLogRepository = new GenericRepository<PredictionLog>(context);
                }
                return predictionLogRepository;
            }
        }
        public GenericRepository<Country> CountryRepository
        {
            get
            {
                if (this.countryRepository == null)
                {
                    this.countryRepository = new GenericRepository<Country>(context);
                }
                return countryRepository;
            }
        }

        public GenericRepository<ApplicationUser> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<ApplicationUser>(context);
                }
                return userRepository;
            }
        }

        public TournamentRepository TournamentRepository
        {
            get
            {
                if (this.tournamentRepository == null)
                {
                    this.tournamentRepository = new TournamentRepository(context);
                }
                return tournamentRepository;
            }
        }

        public PredictionsRepository PredictionsRepository
        {
            get
            {
                if (this.predictionsRepository == null)
                {
                    this.predictionsRepository = new PredictionsRepository(context);
                }
                return predictionsRepository;
            }
        }

        public MatchupRepository MatchupRepository
        {
            get
            {
                if (this.matchupRepository == null)
                {
                    this.matchupRepository = new MatchupRepository(context, TournamentRepository);
                }
                return matchupRepository;
            }
        }




        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
