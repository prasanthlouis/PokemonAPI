using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonAPI.Engines;
using PokemonAPI.FeatureFlags.Providers;
using PokemonAPI.Managers;
using PokemonAPI.Repositories;

namespace AWSServerless1
{
    public class Startup
    {
        private IWebHostEnvironment _env;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IPokemonDetailsManager, PokemonDetailsManager>();
            services.AddTransient<IPokemonDetailsEngine, PokemonDetailsEngine>();
            services.AddTransient<IPokemonDetailsRepository, PokemonDetailsRepository>();
            if(_env.IsDevelopment())
            {
                services.AddTransient<IFeatureFlagProvider, LocalModeFeatureFlagProvider>();
            }
            else
            {
                services.AddTransient<IFeatureFlagProvider, ReleaseFeatureFlagProvider>();
            }

            services.AddTransient<IFeatureFlagProvider, SplitFeatureFlagProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _env = env;
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
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the Pokemon API");
                });
            });
        }
    }
}
