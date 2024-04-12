using Microsoft.Extensions.DependencyInjection;
using HockeyPool.Infrastructure.Data.Repos;

namespace HockeyPool.Infrastructure.Data
{
    public static class DataSetup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<TournamentRepository>();
            services.AddScoped<MatchupRepository>();
            services.AddScoped<PredictionsRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<CountryRepository>();

            return services;
        }     
    }
}
