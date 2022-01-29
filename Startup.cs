using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenaeum_REST_API.Models;
using Athenaeum_REST_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Athenaeum_REST_API
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
            //add CORS
            services.AddCors();

            //DI for BookshelfDatabaseSettings
            services.Configure<BookshelfDatabaseSettings>(Configuration.GetSection(nameof(BookshelfDatabaseSettings)));
            services.AddSingleton<IBookshelfDatabaseSettings>(x => x.GetRequiredService<IOptions<BookshelfDatabaseSettings>>().Value);

            //DI for BookService
            services.AddSingleton<BookService>();
            services.AddSingleton<BookshelfService>();

            //add auth from Auth0
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = domain;
                        options.Audience = Configuration["Auth0:Audience"];
                    });

            //Map Controllers
            services.AddControllers();
        }

        // This method configures the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //middleware which redirects HTTP requests to HTTPS requests
            app.UseHttpsRedirection();

            //middleware which maps HTTP requests to application endpoint in the controller classes
            app.UseRouting();

            //middleware which enables Cross-Origin-Resource-Sharing for requests coming from any origin
            //the allowed origin should be more specific however, since my front-end is not hosted I don't know the origin
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.SetIsOriginAllowed(origin => true) // allow any origin
                //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            //middleware which enable user auth
            app.UseAuthentication();
            app.UseAuthorization();

            //middleware which maps the endpoints in the Controller classes to routes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
