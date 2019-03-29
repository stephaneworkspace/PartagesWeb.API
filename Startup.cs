//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;
using PartagesWeb.API.Data;
using PartagesWeb.API.Helpers;

namespace PartagesWeb.API
{
    /// <summary>
    /// Class Startup pour le démarage de l'application asp.net core
    /// </summary>
    public class Startup
    {
        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary> 
        /// <param name="configuration"> Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>  
        /// Variable de configuration
        /// </summary> 
        public IConfiguration Configuration { get; }

        /// <summary>  
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary> 
        /// <param name="services"> IServiceCollection services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerDocument();
            services.AddCors();
            services.AddAutoMapper();
            // services.AddSingleton<IConfiguration>(Configuration); // BRICOLAGE 5 février > à effacer
            // services.AddScoped<IGestionPagesRepository, GestionPagesRepository>(); BRICOLAGE 5 février > à effacer
            // Seed
            services.AddTransient<Seed>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGestionPagesRepository, GestionPagesRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        /// <summary>  
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary> 
        /// <param name="app"> IApplicationBuilder app</param>
        /// <param name="env"> IHostingEnvironment env</param>
        /// <param name="seeder"> Seed seeder</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            // Seed plus dans les param
            // Desactivé
            // seeder.SeedUsers();
            // seeder.SeedSections();
            // seeder.SeedIcones();
            // seeder.SeedForum();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi3();
        }
    }
}
