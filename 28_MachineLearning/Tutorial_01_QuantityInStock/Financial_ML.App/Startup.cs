using Financial_ML.DAL;
using Financial_ML.MachineLearning;
using Financial_ML.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Financial_ML
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddTransient<IDataProvider, DataProvider>();
            services.AddTransient<IDataSplitter, DataSplitter>();
            services.AddTransient<IDataTrimmer, DataTrimmer>();
            services.AddTransient<IMlRegression, MlRegression>();
            services.AddTransient<IMlBase, MlBase>();
            services.AddTransient<IDataRepository, DataRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Start}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
