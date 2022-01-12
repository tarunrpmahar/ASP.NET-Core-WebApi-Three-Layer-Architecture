using ApiThreeLayerArch.Middlewares;
using DependencyInjection.Configurations.DependencyInjection;
using Domain.Contracts.Configurations;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

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
            services.AddSingleton(Log.Logger);

            services.AddControllers()
                .AddNewtonsoftJson();

            //services.AddDbContext<CommanderDBContext>(opt => opt.UseSqlServer
            //(Configuration.GetConnectionString("CommanderConnectionString")));           

            //services.AddSingleton<CommandProfile>(new CommandProfile());
            //services.AddAutoMapper(typeof(CommandProfile));

            //services.AddScoped<ICommander, SqlCommander>();
            //services.AddScoped<ICommanderRepository, CommanderRepository>();

            services.Configure<TsmConfiguration>(this.Configuration.GetSection("TsmConfiguration"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TsmConfiguration>>().Value);
            var tsmConfiguration = services.BuildServiceProvider().GetService<TsmConfiguration>();
            TsmServiceCollectionExtensions.AddStartupServices(services, tsmConfiguration);
            //services.AddApplicationInsightsTelemetry();

            var instrumentationKey = Configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONKEY")?.Value
                                    ?? Configuration.GetSection("ApplicationInsights:InstrumentationKey")?.Value;

            services.AddApplicationInsightsTelemetry(instrumentationKey);

            services.AddHangfire(x => x.UseSqlServerStorage("<connection string>"));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHangfireDashboard();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
