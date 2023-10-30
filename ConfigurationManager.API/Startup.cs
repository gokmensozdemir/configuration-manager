using ConfigurationManager.API.Cache;
using ConfigurationManager.API.Configs;
using ConfigurationManager.API.Repositories;
using ConfigurationManager.API.Services;
using Identity.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace ConfigurationManager.API
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

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigins",
                                  builder =>
                                  {
                                      builder
                                        .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });

            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

            services.AddScoped<IConfigurationService, ConfigurationService>();

            var redisConnectionString = Configuration.GetValue<string>("RedisConnectionString");

            services.AddSingleton<ICacheService>(new CacheService(redisConnectionString));

            var mongoOptions = Configuration.GetSection("MongoDB").Get<MongoOptions>();

            services.AddSingleton<IMongoClient>(new MongoClient(mongoOptions.ConnectionString));

            services.AddScoped(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(mongoOptions.Database);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors("AllowSpecificOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
