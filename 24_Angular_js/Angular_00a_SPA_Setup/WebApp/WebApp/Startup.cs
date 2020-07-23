using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true)
                    );
            });

            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Map("/apps/app1", builder => {
                builder.UseSpa(spa =>
                {
                    if (env.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer($"http://localhost:4201/");
                    }
                    else
                    {
                        var staticPath = Path.Combine(
                            Directory.GetCurrentDirectory(), $"wwwroot/Apps/dist/app1");
                        var fileOptions = new StaticFileOptions
                        { FileProvider = new PhysicalFileProvider(staticPath) };
                        builder.UseSpaStaticFiles(options: fileOptions);

                        spa.Options.DefaultPageStaticFileOptions = fileOptions;
                    }
                });
            });
            app.UseCors("CorsPolicy");
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
