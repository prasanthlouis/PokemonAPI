using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonAPI.Engines;
using PokemonAPI.Factories.AttackDescription;
using PokemonAPI.FeatureFlags;
using PokemonAPI.FeatureFlags.Providers;
using PokemonAPI.Ifx;
using PokemonAPI.Managers;
using PokemonAPI.Repositories;

namespace PokemonAPI
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;

        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IPokemonDetailsManager, PokemonDetailsManager>();
            services.AddTransient<IPokemonDetailsEngine, PokemonDetailsEngine>();
            services.AddTransient<IPokemonDetailsRepository, PokemonDetailsRepository>();
            if(CurrentEnvironment.IsDevelopment())
            {
                services.AddTransient<IFeatureFlagProvider, LocalModeFeatureFlagProvider>();
            }
            else
            {
                services.AddTransient<IFeatureFlagProvider, ReleaseFeatureFlagProvider>();
            }

            services.AddTransient<IFeatureFlagProvider, SplitFeatureFlagProvider>();
            services.AddTransient<IFeatureAwareFactory, FeatureAwareFactory>();
            services.AddTransient<IAttackDescriptionStrategy, AttackDescriptionManager>();
            services.AddTransient<IAttackDescriptionStrategy, AttackDescriptionManagerDisabled>();
            services.AddTransient<ISplitCreator, SplitCreator>();
            services.AddTransient<IFeatureFlag, FeatureFlag>();
            services.AddTransient<IFeatureFlagTreatment, FeatureFlagTreatment>();
            services.AddTransient<IAttackDescription, AttackDescription>();
            services.AddTransient<IAttackDescriptionFactory, AttackDescriptionFactory>();
            services.AddTransient<IAttackDescriptionFeature, AttackDescriptionFeature>();
            services.Configure<SplitConfigurationOptions>(Configuration.GetSection("SplitConfig"));
            AWSSDKHandler.RegisterXRayForAllServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the Pokemon API");
                });
            });
        }
    }
}
