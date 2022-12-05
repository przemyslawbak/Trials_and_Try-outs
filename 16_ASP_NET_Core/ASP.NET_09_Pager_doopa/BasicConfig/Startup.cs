using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BasicConfig.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace BasicConfig
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                Configuration["Data:BasicConfigApp:ConnectionString"]));
            services.AddTransient<IPanelRepo, PanelRepo>();
            services.AddMvc();
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddPaging();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(
                name: "default", template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Product", action = "Index" });
            });
        }
    }
}
