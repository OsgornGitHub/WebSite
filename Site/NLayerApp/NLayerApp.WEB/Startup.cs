using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLayerApp.BLL.Services;
using StructureMap;
using NLayerApp.BLL.Interfaces;
using NLayerApp.DAL.Interfaces;
using NLayerApp.DAL.Repositories;

namespace NLayerApp.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var container = new Container();

            container.Configure(config =>
            {
                config.AddRegistry(new MyStructuremapRegistry());
                config.Populate(services);

            });


            //container.Configure(config =>
            //{
            //config.AddRegistry(new EFStructuremapRegistry());
            //config.Populate(services);
            //});

            return container.GetInstance<IServiceProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class MyStructuremapRegistry : Registry
    {
        public MyStructuremapRegistry()
        {
            For<IArtistService>().Use<ArtistService>();
            For<IUnitOfWork>().Use<EFUnitOfWork>();
        }
    }



}
