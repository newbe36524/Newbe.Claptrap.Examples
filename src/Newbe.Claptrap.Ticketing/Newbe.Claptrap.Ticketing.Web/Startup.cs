using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newbe.Claptrap.Ticketing.Repository.Module;
using Newbe.Claptrap.Ticketing.Web.Models;
using OpenTelemetry.Trace;

namespace Newbe.Claptrap.Ticketing.Web
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
            services.AddOpenTelemetryTracing(
                builder => builder
                    .AddSource(ClaptrapActivitySource.Instance.Name)
                    .SetSampler(new ParentBasedSampler(new AlwaysOnSampler()))
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddZipkinExporter(options =>
                    {
                        var zipkinBaseUri = Configuration.GetServiceUri("zipkin", "http");
                        options.Endpoint = new Uri(zipkinBaseUri!, "/api/v2/spans");
                    })
            );
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddActors(_ => { });
            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Newbe.Claptrap.Ticketing.Web", Version = "v1"});
            });

            services.AddHttpClient("train", (s, h) =>
            {
                var baseUrl = Configuration.GetServiceUri("ticketing-web");
                h.BaseAddress = baseUrl;
            });
            services.Configure<SiteOptions>(Configuration.GetSection("Ticketing"));
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new RepositoryModule());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] {"zh", "en-US"};
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures.Last())
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Newbe.Claptrap.Ticketing.Web v1"));
            }

            app.UseCors("_AllowTrainOrigin");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });

            app.UseStaticFiles();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}