using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessCentar.data;
using FitnessCentar.data.EF;
using FitnessCentar.service.Interfaces;
using FitnessCentar.service.Services;
using FitnessCentar.web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessCentar.web
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

            services.AddDbContext<MyContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("azure"))            
         );


            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IClanService, ClanService>();
            services.AddTransient<IHelper, Adminsitracija_Helper>();
            services.AddTransient<IPlanIProgramService, PlanIProgramService>();
            services.AddTransient<IWebShopService, WebShopService>();
            services.AddTransient<IZaposlenikService, ZaposlenikService>();
            services.AddTransient<IAjaxService, AjaxService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
