using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BasicConfig.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BasicConfig.Services;

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
            services.AddSession();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IHostedService, TimeHostedTrigger>();
            services.AddScoped<ISomeSerice, SomeSerice>();
            services.AddScoped<IServicesProvider, ServicesProvider>();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes => {
                routes.MapRoute(
                name: "default", template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Product", action = "Index" });
            });
        }
    }
}
