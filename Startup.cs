using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.AdminRepositories;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin;
using UseCase.Admin.PredictorPrices;

namespace DiplomBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            foreach (var line in File.ReadAllLines("./.env"))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DbAppContext>(options => 
            options.UseNpgsql(Configuration.GetConnectionString("DiplomDatabase")));
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<UpdateAppartUseCase, UpdateAppartUseCase>();
            services.AddScoped<PredictorPrice, PredictorPrice>();
            services.AddScoped<CustomPrediction, CustomPrediction>();
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
