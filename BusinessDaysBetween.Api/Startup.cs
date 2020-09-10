using System.IO.Abstractions;
using BusinessDaysBetween.Business.Application;
using BusinessDaysBetween.Business.Commands;
using BusinessDaysBetween.Business.Infrastructure;
using BusinessDaysBetween.Business.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessDaysBetween.Api
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

            // mediatr is probably overkill here, but I like the separation between API and business logic that it 
            // gives us, and avoids unnecessary DI mess into our controllers, less coupling
            services.AddMediatR(typeof(CalculateBusinessDayCommand));

            services.AddSwaggerGen();
            
            // libraries
            services.AddTransient<IFileSystem, FileSystem>(); // default constructor uses default filesystem
            
            // add business logic classes, boilerplate could be isolated in a bigger project for testing?
            services.AddSingleton<IBusinessDayCalculatorService, BusinessDayCalculatorService>();
            services.AddTransient<IHolidayRepository, HolidayRepository>();
            services.AddSingleton<IHolidayFactory, HolidayFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}