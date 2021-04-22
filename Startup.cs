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
using Services.AuthRepository;
using Services.DictionaryRepositories;
using Services.RieltorRepository;
using Services.SearchRepository;
using Usecase.Admin.PredictorPrices;
using UseCase;
using UseCase.Admin;
using UseCase.Auth;
using UseCase.Rieltor;
using UseCase.Search;

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
            //services.AddMvc().AddNewtonsoftJson();
            services.AddMvc();
            services.AddControllers();
            services.AddDbContext<DbAppContext>(options => 
            options.UseNpgsql(Configuration.GetConnectionString("DiplomDatabase")));
            //Repositories
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IRieltorRepository, RieltorRepository>();
            services.AddScoped<ISearchRepository, SearchRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDictionaryRepository, DictionaryRepository>();
            //UseCases
            services.AddScoped<UpdateAppartUseCase, UpdateAppartUseCase>();
            services.AddScoped<PredictPriceUseCase, PredictPriceUseCase>();
            services.AddScoped<SearchUseCase, SearchUseCase>();
            services.AddScoped<InfoUseCase, InfoUseCase>();
            services.AddScoped<SearchPortitableAppsUseCase, SearchPortitableAppsUseCase>();
            services.AddScoped<AuthUseCase, AuthUseCase>();
            services.AddScoped<ImportantPlaceUseCase, ImportantPlaceUseCase>();
            services.AddTransient<PredictorPrice, PredictorPrice>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
