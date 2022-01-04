using DataAccess;
using Domain;
using Domain.Contracts.Configurations;
using Domain.Contracts.Domain;
using Domain.Contracts.Repository;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DependencyInjection.Configurations.DependencyInjection
{
    public class TsmServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupServices(IServiceCollection services, TsmConfiguration tsmConfiguration)
        {
            AddTsmRepositoryServices(services);
            AddTsmDomainServices(services);
            AddTsmDatabase(services, tsmConfiguration);
            AddTsmAutoMapperServices(services);

            return services;
        }

        public static void AddTsmAutoMapperServices(IServiceCollection services)
        {
            services.AddSingleton<CommandProfile>(new CommandProfile());
            services.AddAutoMapper(typeof(CommandProfile));
        }

        public static void AddTsmDomainServices(IServiceCollection services)
        {
            services.AddScoped<ICommander, SqlCommander>();
        }

        public static void AddTsmRepositoryServices(IServiceCollection services)
        {
            services.AddScoped<ICommanderRepository, CommanderRepository>();
        }

        public static void AddTsmDatabase(IServiceCollection services, TsmConfiguration tsmConfiguration)
        {
            var ConnectionString = tsmConfiguration.CommanderConnectionString;
            services.AddDbContext<CommanderDBContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
        }
    }
}
