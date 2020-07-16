using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newbe.Claptrap.Ticketing.Repository.Module;
using Orleans;

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
            services.AddControllers();
            services.AddSwaggerGen(c => { });
            services.AddCors(options =>
            {
                options.AddPolicy("_AllowTrainOrigin", builder =>
                {

                    builder.WithOrigins("http://localhost:52953") //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            AddOrleansClient(services);
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

        private static void AddOrleansClient(IServiceCollection services)
        {
            var clientBuilder = new ClientBuilder();
            var client = clientBuilder
                .UseLocalhostClustering()
                .Build();
            client.Connect(exception =>
            {
                Console.WriteLine(exception);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                return Task.FromResult(true);
            }).Wait();
            services.AddSingleton(client);
            services.AddSingleton<IGrainFactory>(client);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseAuthorization();
            app.UseCors("_AllowTrainOrigin");
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}