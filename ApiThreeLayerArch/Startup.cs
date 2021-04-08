using DataAccess;
using Domain;
using Domain.Contracts;
using Domain.Contracts.Domain;
using Domain.Contracts.Repository;
using Entities.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace ApiThreeLayerArch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContext<CommanderDBContext>(opt => opt.UseSqlServer
            (Configuration.GetConnectionString("CommanderConnectionString")));

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<CommandProfile>(new CommandProfile());
            services.AddAutoMapper(typeof(CommandProfile));

            services.AddScoped<ICommander, SqlCommander>();
            services.AddScoped<ICommanderRepository, CommanderRepository>();

        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}