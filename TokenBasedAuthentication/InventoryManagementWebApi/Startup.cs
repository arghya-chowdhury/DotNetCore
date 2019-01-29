using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
            var key = Encoding.ASCII.GetBytes(Configuration["Token:Secret"]);
            services.AddSingleton<InMemoryDataContext>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime=true,
                    ValidateIssuer=true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
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
