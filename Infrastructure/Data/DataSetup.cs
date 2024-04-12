using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

            return services;
        }     
    }
}
