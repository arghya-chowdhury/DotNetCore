using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public TimeSpan? Timespan { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<InMemoryDataContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options =>
                {
                    options.LoginPath = "/api/Account/Login";
                    options.Cookie.Expiration = TimeSpan.FromMinutes(5);
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerDocument();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
