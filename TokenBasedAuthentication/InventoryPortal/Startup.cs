using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryPortal
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
            services.AddScoped<AccountClient>();
            services.AddScoped<InventoryClient>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = "/Home/Login";
                            options.LogoutPath = "/Home/Login";
                            options.Cookie.Expiration = TimeSpan.FromMinutes(5);
                        });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseMvcWithDefaultRoute();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Oops! Something Went Wrong.");
            });
        }
    }
}