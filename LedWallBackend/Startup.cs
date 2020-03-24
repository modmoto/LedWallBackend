using System;
using LedWallBackend.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedWallBackend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<IPictureRepository, PictureRepository>();

            var mongoConnectionString = _configuration.GetValue<string>("mongoConnectionString");

            var ibimsInfo = _configuration.GetValue<string>("testEnv");
            services.AddSingleton(new DbConnctionInfo(mongoConnectionString?.Replace("'", "")));
            services.AddSingleton(new IBimsInfo(ibimsInfo));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    public class IBimsInfo 
    {
        public string IbimsInfo { get; }

        public IBimsInfo(string ibimsInfo)
        {
            IbimsInfo = ibimsInfo;
        }
    }
}