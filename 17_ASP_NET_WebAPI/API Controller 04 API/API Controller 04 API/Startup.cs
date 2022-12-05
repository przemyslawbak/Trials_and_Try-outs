﻿using System;
using System.IO;
using System.Reflection;
using API_Controller_04_API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;

namespace API_Controller_04_API
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
            //database context
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
            Configuration["Data:appAPI:ConnectionString"]));

            services.AddTransient<IDataRepository, EFDataRepository>();

            services.AddTransient<ISaveFilesRepository, EFSaveFilesRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddXmlDataContractSerializerFormatters()
            .AddMvcOptions(opts => {
                opts.FormatterMappings.SetMediaTypeMappingForFormat("xml",
                new MediaTypeHeaderValue("application/xml"));
                opts.RespectBrowserAcceptHeader = true;
                opts.ReturnHttpNotAcceptable = true;
            });

            services.AddSwaggerGen(c =>
            {
                //The generated Swagger JSON file will have these properties.
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Swagger XML Api Demo",
                    Version = "v1",
                });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();

            //This line enables Swagger UI, which provides us with a nice, simple UI with which we can view our API calls.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger XML Api Demo v1");
            });
            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "api/{controller}/{id}");
            });
        }
    }
}
